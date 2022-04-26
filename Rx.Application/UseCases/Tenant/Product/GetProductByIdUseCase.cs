using MediatR;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Product;

public record GetProductByIdUseCase(Guid Id):IRequest<ProductDto>;

public class GetProductByIdUseCaseHandler : IRequestHandler<GetProductByIdUseCase, ProductDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetProductByIdUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<ProductDto> Handle(GetProductByIdUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductService.GetProductById(request.Id);
    }
}