using Rx.Domain.DTOs.Primary.Organization;

namespace Rx.Domain.Interfaces.Primary;

public interface ISystemSubscriptionService
{
    Task<string> CreateSystemSubscription(SystemSubscriptionForCreationDto subscriptionForCreationDto);
    Task<string> DeactivateSystemSubscription(Guid subscriptionId);
    Task<string> CancelSubscription(Guid subscriptionId);
    Task<string> ActivateOneTimeSubscription(Guid subscriptionId);
    Task<string> ActivateRecurringSubscription(Guid subscriptionId);
    Task<string> RecurringSubscription(Guid subscriptionId);
    Task<string> ActivatePeriodRecurringSubscription(Guid subscriptionId);
}