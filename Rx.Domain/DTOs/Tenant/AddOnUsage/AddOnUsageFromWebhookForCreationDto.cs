namespace Rx.Domain.DTOs.Tenant.AddOnUsage;

public record AddOnUsageFromWebhookForCreationDto(Guid SenderWebhookId,Guid AddOnId,Guid SubscriptionId,int Unit);