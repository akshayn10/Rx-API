using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record ActivatePeriodRecurringSubscriptionUseCase(Guid SubscriptionId):IRequest<string>;

public class ActivatePeriodRecurringSubscriptionUseCaseHandler : IRequestHandler<ActivatePeriodRecurringSubscriptionUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public ActivatePeriodRecurringSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<string> Handle(ActivatePeriodRecurringSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.SubscriptionService.ActivatePeriodRecurringSubscription(request.SubscriptionId);
    }
}