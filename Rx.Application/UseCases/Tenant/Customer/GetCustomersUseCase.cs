using MediatR;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Customer;

public record GetCustomersUseCase():IRequest<IEnumerable<OrganizationCustomerDto>>;

public class GetCustomersUseCaseHandler : IRequestHandler<GetCustomersUseCase, IEnumerable<OrganizationCustomerDto>>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetCustomersUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public  Task<IEnumerable<OrganizationCustomerDto>> Handle(GetCustomersUseCase request, CancellationToken cancellationToken)
    {
        //comment
        return _tenantServiceManager.OrganizationCustomerService.GetCustomers();
    }
}