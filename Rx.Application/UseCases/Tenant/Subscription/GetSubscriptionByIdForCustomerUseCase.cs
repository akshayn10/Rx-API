using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionByIdForCustomerUseCase(Guid CustomerId,Guid SubscriptionId):IRequest<SubscriptionDto>;

public class GetSubscriptionByIdForCustomerUseCaseHandler:IRequestHandler<GetSubscriptionByIdForCustomerUseCase,SubscriptionDto>{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetSubscriptionByIdForCustomerUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }

    public Task<SubscriptionDto> Handle(GetSubscriptionByIdForCustomerUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.SubscriptionService.GetSubscriptionByIdForCustomer(request.CustomerId,request.SubscriptionId);
    }

}