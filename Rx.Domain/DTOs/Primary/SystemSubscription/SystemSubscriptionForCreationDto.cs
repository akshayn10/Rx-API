namespace Rx.Domain.DTOs.Primary.SystemSubscription;

public record SystemSubscriptionForCreationDto(bool SubscriptionType,Guid OrganizationId,Guid SystemSubscriptionPlanId);
