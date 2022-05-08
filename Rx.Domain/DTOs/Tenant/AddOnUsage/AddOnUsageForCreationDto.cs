namespace Rx.Domain.DTOs.Tenant.AddOnUsage;

public record AddOnUsageForCreationDto(DateTime Date,int Unit,Guid AddOnId,Guid SubscriptionId);