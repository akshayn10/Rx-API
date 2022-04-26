using MediatR;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Product;

public record GetProductsUseCase():IRequest<IEnumerable<ProductDto>>;

public class GetProductUseCaseHandler : IRequestHandler<GetProductsUseCase,IEnumerable<ProductDto>>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetProductUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<IEnumerable<ProductDto>> Handle(GetProductsUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductService.GetProducts();
    }
}