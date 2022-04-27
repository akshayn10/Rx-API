using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionsUseCase():IRequest<IEnumerable<SubscriptionDto>>;

public class GetSubscriptionUseCaseHandler : IRequestHandler<GetSubscriptionsUseCase, IEnumerable<SubscriptionDto>>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<IEnumerable<SubscriptionDto>> Handle(GetSubscriptionsUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.SubscriptionService.GetSubscriptions();
    }
}