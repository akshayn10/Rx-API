namespace Rx.Domain.DTOs.Tenant.Subscription;

public record UnsubscriptionWebhookDto(Guid SenderWebhookId,Guid SubscriptionId);