using MediatR;
using Rx.Domain.Interfaces;


namespace Rx.Application.UseCases.Tenant.Webhook;

public record GetWebhookSecretUseCase(Guid productId) : IRequest<string>;

public class GetWebhookSecretUseCaseHandler:IRequestHandler<GetWebhookSecretUseCase,string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetWebhookSecretUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }

    public Task<string> Handle(GetWebhookSecretUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductService.GetWebhookSecret(request.productId);
    }
}
