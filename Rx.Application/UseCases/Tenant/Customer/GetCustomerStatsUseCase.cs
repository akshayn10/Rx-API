using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Customer;

public record GetCustomerStatsUseCase():IRequest<CustomerStatsDto>;

public class GetTotalCustomerUseCaseHandler:IRequestHandler<GetCustomerStatsUseCase,CustomerStatsDto>{
    private readonly ITenantDbContext _tenantDbContext;

    public GetTotalCustomerUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<CustomerStatsDto> Handle(GetCustomerStatsUseCase request, CancellationToken cancellationToken)
    {
        var totalCustomer =await _tenantDbContext.OrganizationCustomers!.CountAsync(cancellationToken: cancellationToken);
        var totalActiveCustomer = await (from c in _tenantDbContext.OrganizationCustomers
            join s in _tenantDbContext.Subscriptions on c.CustomerId equals s.OrganizationCustomerId
            where s.IsActive == true
            select c).Distinct().CountAsync(cancellationToken: cancellationToken);
        var customerStatsDto = new CustomerStatsDto(totalCustomer, totalActiveCustomer);
        return customerStatsDto;
    }
}