using MediatR;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Product;

public record AddProductUseCase(ProductForCreationDto ProductForCreationDto):IRequest<ProductDto>;

public class AddProductUseCaseHandler : IRequestHandler<AddProductUseCase, ProductDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public AddProductUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<ProductDto> Handle(AddProductUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductService.AddProduct(request.ProductForCreationDto);
    }
}