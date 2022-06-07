namespace Rx.Domain.DTOs.Tenant.Subscription;

public record ChangeSubscriptionWebhookDto(Guid SenderWebhookId,Guid SubscriptionId,Guid PlanId,Guid CustomerId,bool NewSubscriptionType);