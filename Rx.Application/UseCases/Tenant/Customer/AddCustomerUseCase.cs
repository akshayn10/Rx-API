using MediatR;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Customer;

public record AddCustomerUseCase(OrganizationCustomerForCreationDto OrganizationCustomerForCreationDto):IRequest<OrganizationCustomerDto>;

public class AddCustomerUseCaseHandler : IRequestHandler<AddCustomerUseCase, OrganizationCustomerDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public AddCustomerUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<OrganizationCustomerDto> Handle(AddCustomerUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.OrganizationCustomerService.AddCustomer(request
            .OrganizationCustomerForCreationDto);
    }
}