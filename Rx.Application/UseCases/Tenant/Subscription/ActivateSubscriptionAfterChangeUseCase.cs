using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record ActivateSubscriptionAfterChangeUseCase(Guid SubscriptionId):IRequest<string>;
public class ActivateSubscriptionAfterChangeUseCaseHandler : IRequestHandler<ActivateSubscriptionAfterChangeUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public ActivateSubscriptionAfterChangeUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<string> Handle(ActivateSubscriptionAfterChangeUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.SubscriptionService.ActivateSubscriptionAfterChange(request.SubscriptionId);
    }
}