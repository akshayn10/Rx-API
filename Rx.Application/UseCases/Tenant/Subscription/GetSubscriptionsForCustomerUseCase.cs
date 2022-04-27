using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionsForCustomerUseCase(Guid CustomerId):IRequest<IEnumerable<SubscriptionDto>>;

public class GetSubscriptionsForCustomerUseCaseHandler : IRequestHandler<GetSubscriptionsForCustomerUseCase, IEnumerable<SubscriptionDto>>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetSubscriptionsForCustomerUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<IEnumerable<SubscriptionDto>> Handle(GetSubscriptionsForCustomerUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.SubscriptionService.GetSubscriptionsForCustomer(request.CustomerId);
    }
}