namespace Rx.Domain.DTOs.Tenant.AddOnUsage;

public record AddOnUsageFromWebhookForCreationDto(Guid SenderAddOnWebhookId,Guid AddOnId,Guid SubscriptionId,int Unit,Guid CustomerId);