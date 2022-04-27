using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record CreateSubscriptionUseCase(SubscriptionForCreationDto SubscriptionForCreationDto):IRequest<SubscriptionDto>;

public class CreateSubscriptionUseCaseHandler : IRequestHandler<CreateSubscriptionUseCase, SubscriptionDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public CreateSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<SubscriptionDto> Handle(CreateSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.SubscriptionService.AddSubscription(request.SubscriptionForCreationDto);
    }
}