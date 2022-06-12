using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record ActivateUpgradeSubscriptionUseCase(Guid WebhookId):IRequest<string>;
public class ActivateUpgradeSubscriptionUseCaseHandler : IRequestHandler<ActivateUpgradeSubscriptionUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public ActivateUpgradeSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<string> Handle(ActivateUpgradeSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.SubscriptionService.ActivateUpgradeSubscription(request.WebhookId);
    }
}