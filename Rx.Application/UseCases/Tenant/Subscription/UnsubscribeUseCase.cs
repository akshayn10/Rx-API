using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record UnsubscribeUseCase(UnsubscriptionWebhookDto UnsubscriptionWebhookDto):IRequest<string>;

public class UnsubscribeUseCaseHandler : IRequestHandler<UnsubscribeUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public UnsubscribeUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<string> Handle(UnsubscribeUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.SubscriptionService.Unsubscribe(request.UnsubscriptionWebhookDto);
    }
}