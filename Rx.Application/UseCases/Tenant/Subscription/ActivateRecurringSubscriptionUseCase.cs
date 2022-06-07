using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record ActivateRecurringSubscriptionUseCase(Guid SubscriptionId):IRequest<string>;

public class ActivateRecurringSubscriptionUseCaseHandler : IRequestHandler<ActivateRecurringSubscriptionUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public ActivateRecurringSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<string> Handle(ActivateRecurringSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.SubscriptionService.ActivateRecurringSubscription(request.SubscriptionId);
    }
}

