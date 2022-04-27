using MediatR;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Product;

public record GetProductsForCustomerUseCase(Guid CustomerId):IRequest<IEnumerable<ProductDto>>;

public class GetProductsForCustomerUseCaseHandler : IRequestHandler<GetProductsForCustomerUseCase, IEnumerable<ProductDto>>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetProductsForCustomerUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<IEnumerable<ProductDto>> Handle(GetProductsForCustomerUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductService.GetProductsForCustomer(request.CustomerId);
    }
}