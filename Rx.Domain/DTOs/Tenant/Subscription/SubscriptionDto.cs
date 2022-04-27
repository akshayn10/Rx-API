namespace Rx.Domain.DTOs.Tenant.Subscription
{
    public record SubscriptionDto(Guid SubscriptionId,DateTime StartDate,DateTime EndDate,bool IsActive,bool IsTrial,DateTime CreatedDate,Guid OrganizationCustomerId,Guid ProductPlanId);
}
