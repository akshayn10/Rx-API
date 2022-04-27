using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionByIdUseCase(Guid Id):IRequest<SubscriptionDto>;

public class GetSubscriptionByIdUseCaseHandler : IRequestHandler<GetSubscriptionByIdUseCase,SubscriptionDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetSubscriptionByIdUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<SubscriptionDto> Handle(GetSubscriptionByIdUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.SubscriptionService.GetSubscriptionById(request.Id);
    }
}