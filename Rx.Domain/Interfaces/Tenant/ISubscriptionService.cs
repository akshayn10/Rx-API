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
        Task<string> CreateSubscriptionFromWebhook (Guid customerId);
        Task<SubscriptionDto> DeactivateSubscription(Guid subscriptionId);
        Task<SubscriptionDto> DeactivateTrialAndActivateSubscription(Guid subscriptionId);
        Task<string> ActivateSubscriptionAfterTrial(Guid subscriptionId);
        Task<string> ActivateOneTimeSubscription(Guid subscriptionId);
        Task<string> RecurringSubscription(Guid subscriptionId);
        Task<string> ActivateRecurringSubscription(Guid subscriptionId);
        Task<string> Unsubscribe(UnsubscriptionWebhookDto unsubscriptionWebhookDto);
        Task<string> UpgradeSubscriptionUseCase(ChangeSubscriptionWebhookDto changeSubscriptionWebhookDto);
        Task<string> DowngradeSubscriptionUseCase(ChangeSubscriptionWebhookDto changeSubscriptionWebhookDto);
        Task<string> ActivateSubscriptionAfterChange(Guid subscriptionId);
    }
}
 