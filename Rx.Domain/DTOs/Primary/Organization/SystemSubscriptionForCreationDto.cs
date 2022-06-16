namespace Rx.Domain.DTOs.Primary.Organization;

public record SystemSubscriptionForCreationDto(bool SubscriptionType,Guid OrganizationId,Guid SystemSubscriptionPlanId);
//DateTime StartDate,DateTime EndDate,bool IsActive,bool IsCancelled,DateTime CreatedDate,