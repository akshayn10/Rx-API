using MediatR;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Product;

public record EditProductUseCase(Guid ProductId,ProductForUpdateDto ProductForUpdateDto) : IRequest<ProductDto>;

public class EditProductUseCaseHandler : IRequestHandler<EditProductUseCase, ProductDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public EditProductUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager=tenantServiceManager ;
    }

    public  Task<ProductDto> Handle(EditProductUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductService.UpdateProduct(request.ProductId,request.ProductForUpdateDto);
    }
}