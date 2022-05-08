namespace Rx.Domain.DTOs.Tenant.AddOnUsage;

public record AddOnUsageDto(Guid AddOnUsageId,DateTime Date,int Unit,Guid AddOnId,Guid SubscriptionId);

