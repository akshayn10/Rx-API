using MediatR;
using Rx.Domain.Interfaces;


namespace Rx.Application.UseCases.Tenant.Webhook;

public record GetWebhookSecretUseCase(Guid productId) : IRequest<Guid>;

public class GetWebhookSecretUseCaseHandler:IRequestHandler<GetWebhookSecretUseCase,Guid>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetWebhookSecretUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }

    public Task<Guid> Handle(GetWebhookSecretUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductService.GetWebhookSecret(request.productId);
    }
}
