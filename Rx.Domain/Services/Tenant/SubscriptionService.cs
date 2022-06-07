using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rx.Domain.DTOs.Payment;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Payment;
using Rx.Domain.Interfaces.Tenant;
namespace Rx.Domain.Services.Tenant
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly ILogger<TenantServiceManager> _logger;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IPaymentService _paymentService;
        private readonly IRecurringJobManager _recurringJobManager;

        public SubscriptionService(ITenantDbContext tenantDbContext, ILogger<TenantServiceManager> logger, IMapper mapper,IBackgroundJobClient backgroundJobClient,IPaymentService paymentService,IRecurringJobManager recurringJobManager)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;
            _paymentService = paymentService;
            _recurringJobManager = recurringJobManager;
        }

        public async Task<IEnumerable<SubscriptionDto>> GetSubscriptions()
        {
            var subscriptions = await _tenantDbContext.Subscriptions!.ToListAsync();
            return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
        }

        public async Task<SubscriptionDto> GetSubscriptionById(Guid id)
        {
            var subscription = await _tenantDbContext.Subscriptions!.FirstOrDefaultAsync(x=>x.SubscriptionId == id);
            return _mapper.Map<SubscriptionDto>(subscription);
        }
        
        //For testing
        public async Task<SubscriptionDto> AddSubscription(SubscriptionForCreationDto subscriptionForCreationDto)
        {
            var subscription = _mapper.Map<Subscription>(subscriptionForCreationDto);
            await _tenantDbContext.Subscriptions!.AddAsync(subscription);
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task<SubscriptionDto> GetSubscriptionByIdForCustomer(Guid customerId, Guid subscriptionId)
        {
            var subscription = await _tenantDbContext.Subscriptions!.FirstOrDefaultAsync(x => x.SubscriptionId == subscriptionId && x.OrganizationCustomerId == customerId);
            return _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task<IEnumerable<SubscriptionDto>> GetSubscriptionsForCustomer(Guid customerId)
        {
            var subscriptions = await _tenantDbContext.Subscriptions!.Where(x => x.OrganizationCustomerId == customerId).ToListAsync();
            return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
        }
        public async Task<SubscriptionDto> DeactivateSubscription(Guid subscriptionId)
        {
            var subscription = await _tenantDbContext.Subscriptions!.FindAsync(subscriptionId);
            subscription!.IsActive = false;
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task<SubscriptionDto> DeactivateTrialAndActivateSubscription(Guid subscriptionId)
        {
            //Deactivate Trial
            var subscription = await _tenantDbContext.Subscriptions!.FindAsync(subscriptionId);
            subscription!.IsTrial = false;
            await _tenantDbContext.SaveChangesAsync();
            
            //Deserialize Description
            var stripeDescription = new StripeDescription("activateAfterTrial", subscription.SubscriptionId.ToString());
            var stripeDescriptionJson = JsonConvert.SerializeObject(stripeDescription,Formatting.Indented);
            
            var customer =await _tenantDbContext.OrganizationCustomers!.FindAsync(subscription.OrganizationCustomerId);
            var plan = await _tenantDbContext.ProductPlans!.FindAsync(subscription.ProductPlanId);
            
            //Processing Payment After Trial
            await _paymentService.Charge(
                customer!.PaymentGatewayId!,
                customer.PaymentMethodId!,
                PaymentModel.Currency.USD,
                Convert.ToInt64(plan!.Price),
                customer.Email!,
                false,
                stripeDescriptionJson
            );
            _logger.LogInformation("Processing Payment");
            return _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task<string> ActivateSubscriptionAfterTrial(Guid subscriptionId)
        {
            var subscription =await _tenantDbContext.Subscriptions!.FindAsync(subscriptionId);
            
            if (subscription is null)
            {
                throw new NullReferenceException("Subscription not found");
            }

            var plan =await _tenantDbContext.ProductPlans!.FindAsync(subscription.ProductPlanId);
            if (plan is null)
            {
                throw new NullReferenceException("Plan not found");
            }
            
            
            subscription.StartDate = DateTime.Now;
            subscription.EndDate=DateTime.Now.AddMonths((int) plan.Duration!);
            subscription.IsActive = true;
            await _tenantDbContext.SaveChangesAsync();
            if (subscription.SubscriptionType==false)
            {
                _backgroundJobClient.Schedule(() => DeactivateSubscription(subscription.SubscriptionId),
                    subscription.StartDate.AddMonths((int) plan.Duration));
                _logger.LogInformation("One TimeSubscription Activated after Trial for " + subscription.SubscriptionId);
            }

            if (subscription.SubscriptionType)
            {
                //Subscription Frequency is Monthly
                _recurringJobManager.AddOrUpdate("jobId",()=>RecurringSubscription(subscription.SubscriptionId),Cron.Monthly());
            }

            return subscription.SubscriptionId.ToString();
        }

        public async Task<string> ActivateOneTimeSubscription(Guid subscriptionId)
        {
            var subscription =await _tenantDbContext.Subscriptions!.FindAsync(subscriptionId);
            
            if (subscription is null)
            {
                throw new NullReferenceException("Subscription not found");
            }

            var plan =await _tenantDbContext.ProductPlans!.FindAsync(subscription.ProductPlanId);
            if (plan is null)
            {
                throw new NullReferenceException("Plan not found");
            }
            
            subscription.IsActive = true;
            await _tenantDbContext.SaveChangesAsync();
            _backgroundJobClient.Schedule(()=>DeactivateSubscription(subscription.SubscriptionId), subscription.StartDate.AddMonths(((int) plan.Duration)!));
            _logger.LogInformation("One Time added Subscription Activated  for "+subscription.SubscriptionId);
            return subscription.SubscriptionId.ToString();
        }

        public async Task<string> RecurringSubscription(Guid subscriptionId)
        {
            var subscription = await _tenantDbContext.Subscriptions!.FindAsync(subscriptionId);
            subscription!.IsActive = false;
            await _tenantDbContext.SaveChangesAsync();
            //Deserialize Description
            var stripeDescription = new StripeDescription("activateRecurringSubscription", subscription.SubscriptionId.ToString());
            var stripeDescriptionJson = JsonConvert.SerializeObject(stripeDescription,Formatting.Indented);
            
            var customer =await _tenantDbContext.OrganizationCustomers!.FindAsync(subscription.OrganizationCustomerId);
            var plan = await _tenantDbContext.ProductPlans!.FindAsync(subscription.ProductPlanId);
            
            //Processing Payment After Trial
            await _paymentService.Charge(
                customer!.PaymentGatewayId!,
                customer.PaymentMethodId!,
                PaymentModel.Currency.USD,
                Convert.ToInt64(plan!.Price),
                customer.Email!,
                false,
                stripeDescriptionJson
            );
            _logger.LogInformation("Processing Payment For Recurring");
            return subscription.SubscriptionId.ToString();
        }

        public async Task<string> ActivateRecurringSubscription(Guid subscriptionId)
        {
            var subscription = await _tenantDbContext.Subscriptions!.FindAsync(subscriptionId);
            if (subscription == null)
            {
                throw new NullReferenceException("Subscription not found at Reactivation");
            }
            
            var plan =await _tenantDbContext.ProductPlans!.FindAsync(subscription.ProductPlanId);
            if (plan == null)
            {
                throw new NullReferenceException("Plan not found at Reactivation");
            }

            if (plan.HaveTrial == false)
            {
                
            }
            
            subscription.IsActive = true;
            await _tenantDbContext.SaveChangesAsync();
            return subscription.SubscriptionId.ToString();
        }

        public async Task<string> CreateSubscriptionFromWebhook(Guid customerId)
        {
            //Get Customer
            var customer =await _tenantDbContext.OrganizationCustomers!.FindAsync(customerId);
            //Get last Webhook for customer
            var webhook = await _tenantDbContext.SubscriptionWebhooks.Where(
                sw=>sw.CustomerEmail == customer!.Email)
                .OrderByDescending(sw=>sw.RetrievedDate)
                .FirstOrDefaultAsync();
            if (webhook is  null)
            {
                throw new Exception("Webhook not found");
            }
            
            //GetPlanDetails
            var plan = await _tenantDbContext.ProductPlans!.FindAsync(webhook!.ProductPlanId);
            //Get Product Details
            var product = await _tenantDbContext.Products!.FindAsync(plan!.ProductId);
            if (product == null || plan == null)
            {
                throw new Exception("Product or Plan not found");
            }
            var planDuration = Convert.ToDouble(plan.Duration);
            
            
            //Check if the plan Have trial
            if (plan.HaveTrial)
            {
                var trialDuration = product.FreeTrialDays;
                //Check if the Subscription is OneTime
                if (webhook.SubscriptionType==false)
                {
                    var subscriptionForCreationDto = new SubscriptionForCreationDto(
                        StartDate:DateTime.Now,
                        EndDate:DateTime.Now.AddDays(trialDuration),
                        IsActive:false,
                        IsTrial: true,
                        CreatedDate:DateTime.Now,
                        OrganizationCustomerId:customer!.CustomerId,
                        ProductPlanId:plan.PlanId,
                        SubscriptionType:webhook.SubscriptionType
                    );
                    var subscription = _mapper.Map<Subscription>(subscriptionForCreationDto);
                    await _tenantDbContext.Subscriptions!.AddAsync(subscription);
                    await _tenantDbContext.SaveChangesAsync();
                    _backgroundJobClient.Schedule(()=>DeactivateTrialAndActivateSubscription(subscription.SubscriptionId), subscription.CreatedDate.AddMinutes(1));
                    
                }
                //Check of the Subscription is Recurring
                if(webhook.SubscriptionType)
                {
                    var subscriptionForCreationDto = new SubscriptionForCreationDto(
                        StartDate:DateTime.Now,
                        EndDate:DateTime.Now.AddDays(trialDuration),
                        IsActive:false,
                        IsTrial: true,
                        CreatedDate:DateTime.Now,
                        OrganizationCustomerId:customer!.CustomerId,
                        ProductPlanId:plan.PlanId,
                        SubscriptionType:webhook.SubscriptionType
                    );
                    var subscription = _mapper.Map<Subscription>(subscriptionForCreationDto);
                    await _tenantDbContext.Subscriptions!.AddAsync(subscription);
                    await _tenantDbContext.SaveChangesAsync();
                    _backgroundJobClient.Schedule(()=>DeactivateTrialAndActivateSubscription(subscription.SubscriptionId), subscription.CreatedDate.AddMinutes(1));
                }
            }
            //Check if the plan has no Trial
            if (plan.HaveTrial == false)
            {
                var subscriptionForCreationDto = new SubscriptionForCreationDto(
                    StartDate:DateTime.Now,
                    EndDate:DateTime.Now.AddDays(planDuration),
                    IsActive:false,
                    IsTrial: false,
                    CreatedDate:DateTime.Now,
                    OrganizationCustomerId:customer!.CustomerId,
                    ProductPlanId:plan.PlanId,
                    SubscriptionType:webhook.SubscriptionType
                );
                var subscription = _mapper.Map<Subscription>(subscriptionForCreationDto);
                await _tenantDbContext.Subscriptions!.AddAsync(subscription);
                await _tenantDbContext.SaveChangesAsync();
                
                //Check if the Subscription is One Time
                if (webhook.SubscriptionType == false)
                {
                    var stripeDescription = new StripeDescription("activateOneTimeSubscription", subscription.SubscriptionId.ToString());
                    var stripeDescriptionJson = JsonConvert.SerializeObject(stripeDescription,Formatting.Indented);
                    
            
                    //Processing Payment After Trial
                    await _paymentService.Charge(
                        customer!.PaymentGatewayId!,
                        customer.PaymentMethodId!,
                        PaymentModel.Currency.USD,
                        Convert.ToInt64(plan!.Price),
                        customer.Email!,
                        false,
                        stripeDescriptionJson
                    );
                    
                    _logger.LogInformation("Processing Payment");

                    _backgroundJobClient.Schedule(()=>DeactivateTrialAndActivateSubscription(subscription.SubscriptionId), subscription.CreatedDate.AddMinutes(1));
                }
                //Check of the Subscription is Recurring
                if (webhook.SubscriptionType)
                {
                    _recurringJobManager.AddOrUpdate("jobId",()=>RecurringSubscription(subscription.SubscriptionId),Cron.Monthly());
                    _logger.LogInformation("Processing Payment");
                }
            }

            return "";
        }
    }
}
