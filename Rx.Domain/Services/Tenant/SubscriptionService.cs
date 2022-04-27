using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public SubscriptionService(ITenantDbContext tenantDbContext, ILogger logger, IMapper mapper)
        {
            _tenantDbContext = tenantDbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubscriptionDto>> GetSubscriptions()
        {
            var subscriptions = await _tenantDbContext.Subscriptions.ToListAsync();
            return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
        }

        public async Task<SubscriptionDto> GetSubscriptionById(Guid id)
        {
            var subscription = await _tenantDbContext.Subscriptions.FirstOrDefaultAsync(x=>x.SubscriptionId == id);
            return _mapper.Map<SubscriptionDto>(subscription);
        }
        
        //For testing
        public async Task<SubscriptionDto> AddSubscription(SubscriptionForCreationDto subscriptionForCreationDto)
        {
            var subscription = _mapper.Map<Subscription>(subscriptionForCreationDto);
            await _tenantDbContext.Subscriptions.AddAsync(subscription);
            await _tenantDbContext.SaveChangesAsync();
            return _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task<SubscriptionDto> GetSubscriptionByIdForCustomer(Guid customerId, Guid subscriptionId)
        {
            var subscription = await _tenantDbContext.Subscriptions.FirstOrDefaultAsync(x => x.SubscriptionId == subscriptionId && x.OrganizationCustomerId == customerId);
            return _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task<IEnumerable<SubscriptionDto>> GetSubscriptionsForCustomer(Guid customerId)
        {
            var subscriptions = await _tenantDbContext.Subscriptions.Where(x => x.OrganizationCustomerId == customerId).ToListAsync();
            return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
        }
    }
}
