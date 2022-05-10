using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record DeactivateSubscriptionUseCase(Guid SubscriptionId):IRequest<SubscriptionDto>;

public class DeactivateSubscriptionUseCaseHandler : IRequestHandler<DeactivateSubscriptionUseCase,SubscriptionDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public DeactivateSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<SubscriptionDto> Handle(DeactivateSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.SubscriptionService.DeactivateeSubscription(request.SubscriptionId);
    }
}