using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record UpgradeSubscriptionUseCase(ChangeSubscriptionWebhookDto ChangeSubscriptionWebhookDto):IRequest<string>;

public class UpgradeSubscriptionUseCaseHandler : IRequestHandler<UpgradeSubscriptionUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public UpgradeSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<string> Handle(UpgradeSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.SubscriptionService.UpgradeSubscriptionUseCase(request.ChangeSubscriptionWebhookDto);
    }
}

