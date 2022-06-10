using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.DTOs.Tenant.Subscription;

namespace Rx.Domain.Interfaces.WebhookSendClient;

public interface ISendWebhookService
{
    Task SendSubscriptionWebhookToProductBackend(BackendSubscriptionResponse backendSubscriptionResponse);
    Task SendAddOnWebhookToProductBackend(BackendAddOnResponse backendAddOnResponse);
}