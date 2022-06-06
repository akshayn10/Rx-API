namespace Rx.Domain.DTOs.Tenant.AddOnUsage;

public record AddOnWebhookDto(Guid SenderWebhookId,Guid AddOnId,Guid SubscriptionId,int Unit,DateTime RetrievedDateTime);