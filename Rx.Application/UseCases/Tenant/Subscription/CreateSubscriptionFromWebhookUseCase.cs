using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record CreateSubscriptionFromWebhookUseCase(Guid WebhookId):IRequest<string>;

public class CreateSubscriptionFromWebhookUseCaseHandler : IRequestHandler<CreateSubscriptionFromWebhookUseCase,string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public CreateSubscriptionFromWebhookUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<string> Handle(CreateSubscriptionFromWebhookUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.SubscriptionService.CreateSubscriptionFromWebhook(request.WebhookId);
    }
}