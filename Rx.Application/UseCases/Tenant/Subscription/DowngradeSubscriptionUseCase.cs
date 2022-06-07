using Rx.Domain.DTOs.Tenant.Subscription;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record DowngradeSubscriptionUseCase(ChangeSubscriptionWebhookDto ChangeSubscriptionWebhookDto);
