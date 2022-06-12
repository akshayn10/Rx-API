using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record ActivateDowngradeSubscriptionUseCase(Guid WebhookId):IRequest<string>;

public class ActivateDowngradeSubscriptionUseCaseHandler : IRequestHandler<ActivateDowngradeSubscriptionUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public ActivateDowngradeSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<string> Handle(ActivateDowngradeSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.SubscriptionService.ActivateDowngradeSubscription(request.WebhookId);
    }
}

