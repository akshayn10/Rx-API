namespace Rx.Domain.DTOs.Tenant.Subscription
{
    public record SubscriptionForCreationDto(DateTime StartDate, DateTime EndDate, bool IsActive, bool IsTrial,
        DateTime CreatedDate, Guid OrganizationCustomerId, Guid ProductPlanId,bool SubscriptionType);

}
