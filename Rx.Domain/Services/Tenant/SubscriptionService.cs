using System.ComponentModel;
using AutoMapper;
using Hangfire;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;
namespace Rx.Domain.Services.Tenant
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ITenantDbContext _tenantDbContext;
        private readonly ILogger<TenantServiceManager> _logger;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public SubscriptionService(ITenantDbContext tenantDbContext, ILogger<TenantServiceManager> logger, IMapper mapper,IBackgroundJobClient backgroundJobClient)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;
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
        public async Task<SubscriptionDto> DeactivateeSubscription(Guid subscriptionId)
        {
            var subscription = await _tenantDbContext.Subscriptions!.FindAsync(subscriptionId);
            subscription!.IsActive = false;
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<SubscriptionDto>(subscription);
        }
        public async Task<SubscriptionDto> CreateSubscriptionFromWebhook(SubscriptionWebhookForCreationDto subscriptionWebhookForCreationDto)
        {
            var customer =await _tenantDbContext.OrganizationCustomers!.FirstOrDefaultAsync(c=>c.Email == subscriptionWebhookForCreationDto.customerEmail);
            if (customer is null)
            {
                var customerForCreationDto = new OrganizationCustomerForCreationDto(
                    Email: subscriptionWebhookForCreationDto.customerEmail,
                    Name: subscriptionWebhookForCreationDto.customerName
                );
                //Create New Customer
                customer = _mapper.Map<OrganizationCustomer>(customerForCreationDto);
                await _tenantDbContext.OrganizationCustomers!.AddAsync(customer);
                await _tenantDbContext.SaveChangesAsync();
                
            }
            //GetPlanDetails
            var plan = await _tenantDbContext.ProductPlans!.FindAsync(subscriptionWebhookForCreationDto.productPlanId);
            var product = await _tenantDbContext.Products!.FindAsync(plan!.ProductId);
            if (product == null || plan == null)
            {
                throw new Exception("Product or Plan not found");
            }
            var planDuration = Convert.ToDouble(plan.Duration);
            //Create Subscription
            var subscriptionForCreationDto = new SubscriptionForCreationDto(
                StartDate:DateTime.Now,
                EndDate:DateTime.Now.AddDays(planDuration),
                IsActive:true,
                IsTrial: (product!.FreeTrialDays > 0),
                CreatedDate:DateTime.Now,
                OrganizationCustomerId:customer.CustomerId,
                ProductPlanId:plan.PlanId
            );
            var subscription = _mapper.Map<Subscription>(subscriptionForCreationDto);
            await _tenantDbContext.Subscriptions!.AddAsync(subscription);
            await _tenantDbContext.SaveChangesAsync();
            // _backgroundJobClient.Schedule(()=>DeactivateSubscription(subscription.SubscriptionId), subscription.EndDate);
            _backgroundJobClient.Schedule(()=>DeactivateeSubscription(subscription.SubscriptionId), subscription.CreatedDate.AddMinutes(1));
            return _mapper.Map<SubscriptionDto>(subscription);

        }
    }
}
