using MediatR;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Product;

public record GetCustomersForProductUseCase(Guid ProductId):IRequest<IEnumerable<OrganizationCustomerDto>>;

public class
    GetCustomersForProductUseCaseHandler : IRequestHandler<GetCustomersForProductUseCase,IEnumerable<OrganizationCustomerDto>>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetCustomersForProductUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<IEnumerable<OrganizationCustomerDto>> Handle(GetCustomersForProductUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductService.GetCustomersForProduct(request.ProductId);
    }
}