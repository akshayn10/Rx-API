namespace Rx.Domain.DTOs.Tenant.Subscription;

public record SubscriptionWebhookForCreationDto(Guid SenderWebhookId,string customerEmail,string customerName,Guid productPlanId);

 