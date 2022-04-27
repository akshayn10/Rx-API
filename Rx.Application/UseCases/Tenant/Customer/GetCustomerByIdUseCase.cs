using MediatR;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Customer;

public record GetCustomerByIdUseCase(Guid Id):IRequest<OrganizationCustomerDto>;

public class GetCustomerByIdUseCaseHandler : IRequestHandler<GetCustomerByIdUseCase,OrganizationCustomerDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetCustomerByIdUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<OrganizationCustomerDto> Handle(GetCustomerByIdUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.OrganizationCustomerService.GetCustomerById(request.Id);
    }
}