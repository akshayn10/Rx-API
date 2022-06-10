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
using Rx.Domain.Interfaces.WebhookSendClient;

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
        private readonly ISendWebhookService _sendWebhookService;
        
        public SubscriptionService(
            ITenantDbContext tenantDbContext,
            ILogger<TenantServiceManager> logger,
            IMapper mapper,
            IBackgroundJobClient backgroundJobClient,
            IPaymentService paymentService,
            IRecurringJobManager recurringJobManager,
            ISendWebhookService sendWebhookService
            )
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;
            _paymentService = paymentService;
            _recurringJobManager = recurringJobManager;
            _sendWebhookService = sendWebhookService;
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
            var subscription = await _tenantDbContext.Subscriptions!.FirstOrDefaultAsync(
                x => x.SubscriptionId == subscriptionId && x.OrganizationCustomerId == customerId);
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
            
            //Response to Backend
            var backendSubscriptionResponse = new BackendSubscriptionResponse(
                "DeActivatedTrial",
                subscription.SubscriptionId.ToString(),
                subscription.OrganizationCustomerId.ToString(),
                subscription.ProductPlanId.ToString());
            await _sendWebhookService.SendSubscriptionWebhookToProductBackend(backendSubscriptionResponse);
            
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
                var jobId = Guid.NewGuid().ToString("N");
                var subscriptionUpdate =await _tenantDbContext.Subscriptions!.FindAsync(subscription.SubscriptionId);
                if(subscriptionUpdate is null)
                {
                    throw new NullReferenceException("Subscription not found");
                }
                subscriptionUpdate!.JobId = jobId;
                //Subscription Frequency is Monthly
                _recurringJobManager.AddOrUpdate(jobId,()=>RecurringSubscription(subscription.SubscriptionId),Cron.Monthly());
            }
            var backendSubscriptionResponse = new BackendSubscriptionResponse(
                "ActivatedSubscription",subscription.SubscriptionId.ToString(),subscription.OrganizationCustomerId.ToString(),subscription.ProductPlanId.ToString());
            await _sendWebhookService.SendSubscriptionWebhookToProductBackend(backendSubscriptionResponse);
            _logger.LogInformation("Subscription Activated after Trial for " + subscription.SubscriptionId);

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
            
            var backendSubscriptionResponse = new BackendSubscriptionResponse(
                "ActivatedSubscription",
                subscription.SubscriptionId.ToString(),
                subscription.OrganizationCustomerId.ToString(),
                subscription.ProductPlanId.ToString());
            await _sendWebhookService.SendSubscriptionWebhookToProductBackend(backendSubscriptionResponse);
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
                throw new NullReferenceException("Subscription not found at activation");
            }
            
            var plan =await _tenantDbContext.ProductPlans!.FindAsync(subscription.ProductPlanId);
            if (plan == null)
            {
                throw new NullReferenceException("Plan not found at activation");
            }
            
            subscription.IsActive = true;
            await _tenantDbContext.SaveChangesAsync();
            
            var backendSubscriptionResponse = new BackendSubscriptionResponse(
                "ActivatedSubscription",
                subscription.SubscriptionId.ToString(),
                subscription.OrganizationCustomerId.ToString(),
                subscription.ProductPlanId.ToString());
            
            await _sendWebhookService.SendSubscriptionWebhookToProductBackend(backendSubscriptionResponse);
            
            _logger.LogInformation("Recurring Subscription Activated for " + subscription.SubscriptionId);
            return subscription.SubscriptionId.ToString();
        }

        public async Task<string> Unsubscribe(UnsubscriptionWebhookDto unsubscriptionWebhookDto)
        {
            var subscription = await _tenantDbContext.Subscriptions!.FindAsync(unsubscriptionWebhookDto.SubscriptionId);
            if (subscription == null)
            {
                throw new NullReferenceException("Subscription not found at Unsubscription");
            }
            subscription.IsActive = false;
            subscription.IsCancelled = true;
            await _tenantDbContext.SaveChangesAsync();

            //Cancel Recurring job for recurring subscription
            if (subscription.SubscriptionType)
            {
                _recurringJobManager.RemoveIfExists(subscription.JobId);
            }

            var plan =await _tenantDbContext.ProductPlans!.FindAsync(subscription.ProductPlanId);

            var subStat = new SubscriptionStat()
            {
                Change = "unsubscribe",
                SubscriptionId = subscription.SubscriptionId,
                Date = DateTime.Now,
                ProductId = plan!.ProductId
            };
            await _tenantDbContext.SubscriptionStats!.AddAsync(subStat);
            await _tenantDbContext.SaveChangesAsync();
            
          
            var backendSubscriptionResponse = new BackendSubscriptionResponse(
                "unsubscribe",
                subscription.SubscriptionId.ToString(),
                subscription.OrganizationCustomerId.ToString(),
                subscription.ProductPlanId.ToString());
            
            await _sendWebhookService.SendSubscriptionWebhookToProductBackend(backendSubscriptionResponse);
            _logger.LogInformation("Subscription Cancelled for " + unsubscriptionWebhookDto.SubscriptionId);
            return "Subscription Cancelled";
        }

        public async Task<string> UpgradeSubscriptionUseCase(ChangeSubscriptionWebhookDto changeSubscriptionWebhookDto)
        {
            //Cancel old Subscription
             var unsubscriptionDto=new UnsubscriptionWebhookDto(changeSubscriptionWebhookDto.SenderWebhookId,changeSubscriptionWebhookDto.SubscriptionId);
             await Unsubscribe(unsubscriptionDto);
             
            var customer=await _tenantDbContext.OrganizationCustomers!.FindAsync(changeSubscriptionWebhookDto.CustomerId);
            if(customer is null)
            {
                throw new NullReferenceException("Customer not found");
            }
            var plan=await _tenantDbContext.ProductPlans!.FindAsync(changeSubscriptionWebhookDto.PlanId);
            if(plan is null)
            {
                throw new NullReferenceException("Plan not found");
            }

            var planDuration = Convert.ToDouble(plan.Duration);
            var subscriptionForCreationDto = new SubscriptionForCreationDto(
                StartDate:DateTime.Now,
                EndDate:DateTime.Now.AddDays(planDuration),
                IsActive:false,
                IsTrial: false,
                CreatedDate:DateTime.Now,
                OrganizationCustomerId:changeSubscriptionWebhookDto.CustomerId,
                ProductPlanId:changeSubscriptionWebhookDto.PlanId,
                SubscriptionType:changeSubscriptionWebhookDto.NewSubscriptionType
            );
            var subscription = _mapper.Map<Subscription>(subscriptionForCreationDto);
            await _tenantDbContext.Subscriptions!.AddAsync(subscription);
            await _tenantDbContext.SaveChangesAsync();
            
            // Update SubscriptionStats
            var subStat = new SubscriptionStat()
            {
                Change = "upgrade",
                SubscriptionId = subscription.SubscriptionId,
                Date = DateTime.Now,
                ProductId = plan!.ProductId
            };
            await _tenantDbContext.SubscriptionStats!.AddAsync(subStat);
            await _tenantDbContext.SaveChangesAsync();
            var stripeDescription = new StripeDescription("changeSubscription", subscription.SubscriptionId.ToString());
            var stripeDescriptionJson = JsonConvert.SerializeObject(stripeDescription,Formatting.Indented);
            
            await _paymentService.Charge(
                customer!.PaymentGatewayId!,
                customer.PaymentMethodId!,
                PaymentModel.Currency.USD,
                Convert.ToInt64(plan!.Price),
                customer.Email!,
                false,
                stripeDescriptionJson
            );
            
            return "Payment processig for Upgrade Subscription";
            
        }
        public async Task<string> DowngradeSubscriptionUseCase(ChangeSubscriptionWebhookDto changeSubscriptionWebhookDto)
        {
            //Cancel old Subscription
             var unsubscriptionDto=new UnsubscriptionWebhookDto(changeSubscriptionWebhookDto.SenderWebhookId,changeSubscriptionWebhookDto.SubscriptionId);
             await Unsubscribe(unsubscriptionDto);
             
             
             
            var customer=await _tenantDbContext.OrganizationCustomers!.FindAsync(changeSubscriptionWebhookDto.CustomerId);
            if(customer is null)
            {
                throw new NullReferenceException("Customer not found");
            }
            var plan=await _tenantDbContext.ProductPlans!.FindAsync(changeSubscriptionWebhookDto.PlanId);
            if(plan is null)
            {
                throw new NullReferenceException("Plan not found");
            }

            var planDuration = Convert.ToDouble(plan.Duration);
            var subscriptionForCreationDto = new SubscriptionForCreationDto(
                StartDate:DateTime.Now,
                EndDate:DateTime.Now.AddDays(planDuration),
                IsActive:false,
                IsTrial: false,
                CreatedDate:DateTime.Now,
                OrganizationCustomerId:changeSubscriptionWebhookDto.CustomerId,
                ProductPlanId:changeSubscriptionWebhookDto.PlanId,
                SubscriptionType:changeSubscriptionWebhookDto.NewSubscriptionType
            );
            var subscription = _mapper.Map<Subscription>(subscriptionForCreationDto);
            await _tenantDbContext.Subscriptions!.AddAsync(subscription);
            await _tenantDbContext.SaveChangesAsync();
            
            // Update SubscriptionStats
            var subStat = new SubscriptionStat()
            {
                Change = "downgrade",
                SubscriptionId = subscription.SubscriptionId,
                Date = DateTime.Now,
                ProductId = plan!.ProductId
            };
            await _tenantDbContext.SubscriptionStats!.AddAsync(subStat);
            await _tenantDbContext.SaveChangesAsync();
            
            var stripeDescription = new StripeDescription("changeSubscription", subscription.SubscriptionId.ToString());
            var stripeDescriptionJson = JsonConvert.SerializeObject(stripeDescription,Formatting.Indented);
            
            await _paymentService.Charge(
                customer!.PaymentGatewayId!,
                customer.PaymentMethodId!,
                PaymentModel.Currency.USD,
                Convert.ToInt64(plan!.Price),
                customer.Email!,
                false,
                stripeDescriptionJson
            );
            
            return "Payment processig for Upgrade Subscription";
            
        }

        public async Task<string> ActivateSubscriptionAfterChange(Guid subscriptionId)
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
            
            // subscription.StartDate = DateTime.Now;
            // subscription.EndDate=DateTime.Now.AddMonths((int) plan.Duration!);
            subscription.IsActive = true;
            await _tenantDbContext.SaveChangesAsync();
            if (subscription.SubscriptionType==false)
            {
                _backgroundJobClient.Schedule(() => DeactivateSubscription(subscription.SubscriptionId),
                    subscription.StartDate.AddMonths((int) plan.Duration));
                _logger.LogInformation("One TimeSubscription Activated Change " + subscription.SubscriptionId);
            }

            if (subscription.SubscriptionType)
            {
                var jobId = Guid.NewGuid().ToString("N");
                var subscriptionUpdate =await _tenantDbContext.Subscriptions!.FindAsync(subscription.SubscriptionId);
                if(subscriptionUpdate is null)
                {
                    throw new NullReferenceException("Subscription not found");
                }
                subscriptionUpdate!.JobId = jobId;
                await _tenantDbContext.SaveChangesAsync();
                //Subscription Frequency is Monthly
                _recurringJobManager.AddOrUpdate(jobId,()=>RecurringSubscription(subscription.SubscriptionId),Cron.Monthly());
               
                var backendSubscriptionResponse = new BackendSubscriptionResponse(
                    "ActivatedSubscription",subscription.SubscriptionId.ToString(),subscription.OrganizationCustomerId.ToString(),subscription.ProductPlanId.ToString());
                await _sendWebhookService.SendSubscriptionWebhookToProductBackend(backendSubscriptionResponse);
                _logger.LogInformation("Recurring subscription Activated Change " + subscription.SubscriptionId);
            }

            return subscription.SubscriptionId.ToString();
            
        }

        public async Task<string> CreateSubscriptionFromWebhook(Guid webhookId)
        {
            
            //Get last Webhook for customer
            var webhook = await _tenantDbContext.SubscriptionWebhooks.FindAsync(webhookId);
            if (webhook is  null)
            {
                throw new Exception("Webhook not found");
            }
            //Get Customer
            var customer =await _tenantDbContext.OrganizationCustomers!.FirstOrDefaultAsync(c=>c.Email==webhook.CustomerEmail);
            if (customer is null)
            {
                throw new NullReferenceException("Customer not found");
            }
            
            //GetPlanDetails
            var plan = await _tenantDbContext.ProductPlans!.FindAsync(webhook!.ProductPlanId);
            if (plan is null)
            {
                throw new NullReferenceException("Plan not found");
            }
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
                    var backendSubscriptionResponse = new BackendSubscriptionResponse(
                        "ActivatedTrial",
                        subscription.SubscriptionId.ToString(),
                        subscription.OrganizationCustomerId.ToString(),
                        subscription.ProductPlanId.ToString());
                    await _sendWebhookService.SendSubscriptionWebhookToProductBackend(backendSubscriptionResponse);
                    
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
                    
                    
                    var backendSubscriptionResponse = new BackendSubscriptionResponse(
                        "ActivatedTrial",subscription.SubscriptionId.ToString(),subscription.OrganizationCustomerId.ToString(),subscription.ProductPlanId.ToString());
                    await _sendWebhookService.SendSubscriptionWebhookToProductBackend(backendSubscriptionResponse);
                    return "Activated Trial";
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

                    _backgroundJobClient.Schedule(()=>DeactivateSubscription(subscription.SubscriptionId), subscription.CreatedDate.AddMinutes(1));
                    return "Payment processing for One Time Subscription without trial";
                }
                //Check of the Subscription is Recurring
                if (webhook.SubscriptionType)
                {
                    var jobId = Guid.NewGuid().ToString("N");
                    var subscriptionUpdate =await _tenantDbContext.Subscriptions!.FindAsync(subscription.SubscriptionId);
                    if(subscriptionUpdate is null)
                    {
                        throw new NullReferenceException("Subscription not found");
                    }
                    subscriptionUpdate!.JobId = jobId;
                    await _tenantDbContext.SaveChangesAsync();
                    _recurringJobManager.AddOrUpdate(jobId,()=>RecurringSubscription(subscription.SubscriptionId),Cron.Monthly());
                    _logger.LogInformation("Processing Payment");
                    return "Payment processing for Recurring Subscription without trial";
                }
            }
            return "Unhandled Error on Subscription Creation";
        }
    }
}