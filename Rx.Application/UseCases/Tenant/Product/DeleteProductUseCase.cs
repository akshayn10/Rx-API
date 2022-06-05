using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Product;

public record DeleteProductUseCase(Guid ProductId):IRequest<string>;

public class DeleteProductUseCaseHandler : IRequestHandler<DeleteProductUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public DeleteProductUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }

    public async Task<string> Handle(DeleteProductUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.ProductService.DeleteProduct(request.ProductId);
    }
}