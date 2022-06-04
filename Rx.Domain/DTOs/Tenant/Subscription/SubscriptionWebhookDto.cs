namespace Rx.Domain.DTOs.Tenant.Subscription;

public record SubscriptionWebhookDto(Guid SenderWebhookId,string CustomerEmail,string CustomerName,Guid ProductPlanId);