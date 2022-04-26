using MediatR;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Customer;

public record GetCustomerStatsUseCase():IRequest<CustomerStatsDto>;

public class GetTotalCustomerUseCaseHandler:IRequestHandler<GetCustomerStatsUseCase,CustomerStatsDto>{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetTotalCustomerUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    
    public Task<CustomerStatsDto> Handle(GetCustomerStatsUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.OrganizationCustomerService.GetCustomerStats();
    }
}