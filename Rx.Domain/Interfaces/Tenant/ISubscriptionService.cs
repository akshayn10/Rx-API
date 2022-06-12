using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Entities.Tenant;

namespace Rx.Domain.Interfaces.Tenant
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionDto>> GetSubscriptions();
        Task<SubscriptionDto> GetSubscriptionById(Guid id);
        Task<SubscriptionDto> AddSubscription(SubscriptionForCreationDto subscriptionForCreationDto);
        Task<SubscriptionDto> GetSubscriptionByIdForCustomer(Guid customerId, Guid subscriptionId);
        Task<IEnumerable<SubscriptionDto>> GetSubscriptionsForCustomer(Guid customerId);
        Task<string> CreateSubscriptionFromWebhook (Guid webhookId);
        Task<SubscriptionDto> DeactivateSubscription(Guid subscriptionId);
        Task<SubscriptionDto> DeactivateTrialAndActivateSubscription(Guid subscriptionId);
        Task<string> ActivateSubscriptionAfterTrial(Guid webhookId);
        Task<string> ActivateOneTimeSubscription(Guid webhookId);
        Task<string> RecurringSubscription(Guid subscriptionId);
        Task<string> ActivateRecurringSubscription(Guid subscriptionId);
        Task<string> Unsubscribe(UnsubscriptionWebhookDto unsubscriptionWebhookDto);
        Task<string> UpgradeSubscription(ChangeSubscriptionWebhook changeSubscriptionWebhook);
        Task<string> DowngradeSubscription(ChangeSubscriptionWebhook changeSubscriptionWebhook);
        Task<string> ActivateUpgradeSubscription(Guid webhookId);
        Task<string> ActivateDowngradeSubscription(Guid webhookId);
        Task<string> ActivatePeriodRecurringSubscription(Guid subscriptionId);
        
    }
}
 