using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record ActivateOneTimeSubscriptionUseCase(Guid SubscriptionId):IRequest<string>;

public class ActivateOneTimeSubscriptionUseCaseHandler : IRequestHandler<ActivateOneTimeSubscriptionUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public ActivateOneTimeSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<string> Handle(ActivateOneTimeSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.SubscriptionService.ActivateOneTimeSubscription(request.SubscriptionId);
    }
}