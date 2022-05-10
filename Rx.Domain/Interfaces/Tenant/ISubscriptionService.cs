using Rx.Domain.DTOs.Tenant.Subscription;

namespace Rx.Domain.Interfaces.Tenant
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionDto>> GetSubscriptions();
        Task<SubscriptionDto> GetSubscriptionById(Guid id);
        Task<SubscriptionDto> AddSubscription(SubscriptionForCreationDto subscriptionForCreationDto);
        Task<SubscriptionDto> GetSubscriptionByIdForCustomer(Guid customerId, Guid subscriptionId);
        Task<IEnumerable<SubscriptionDto>> GetSubscriptionsForCustomer(Guid customerId);
        
        Task<SubscriptionDto> CreateSubscriptionFromWebhook (SubscriptionWebhookForCreationDto subscriptionWebhookForCreationDto);
        Task<SubscriptionDto> DeactivateeSubscription(Guid subscriptionId);
    }
}
