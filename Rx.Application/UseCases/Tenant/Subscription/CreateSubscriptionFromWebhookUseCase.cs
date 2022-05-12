using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record CreateSubscriptionFromWebhookUseCase(SubscriptionWebhookForCreationDto SubscriptionWebhookForCreationDto):IRequest<SubscriptionDto>;

public class CreateSubscriptionFromWebhookUseCaseHandler : IRequestHandler<CreateSubscriptionFromWebhookUseCase, SubscriptionDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public CreateSubscriptionFromWebhookUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<SubscriptionDto> Handle(CreateSubscriptionFromWebhookUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.SubscriptionService.CreateSubscriptionFromWebhook(request.SubscriptionWebhookForCreationDto);
        
    }
}