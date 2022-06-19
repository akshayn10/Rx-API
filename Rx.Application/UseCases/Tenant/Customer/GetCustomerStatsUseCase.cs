using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Customer;

public record GetCustomerStatsUseCase():IRequest<CustomerStatsVm>;

public class GetTotalCustomerUseCaseHandler:IRequestHandler<GetCustomerStatsUseCase,CustomerStatsVm>{
    private readonly ITenantDbContext _tenantDbContext;

    public GetTotalCustomerUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<CustomerStatsVm> Handle(GetCustomerStatsUseCase request, CancellationToken cancellationToken)
    {
        var totalCustomer =await _tenantDbContext.OrganizationCustomers!.CountAsync(cancellationToken: cancellationToken);
        var totalActiveCustomer = await _tenantDbContext.OrganizationCustomers!.Include(c=>c.Subscriptions)
            .Where(c=>c.Subscriptions!.Any(s=>s.IsActive)).CountAsync(cancellationToken: cancellationToken);
        var totalInactiveCustomer = totalCustomer - totalActiveCustomer;
        return new CustomerStatsVm(
            totalCustomer,totalActiveCustomer,totalInactiveCustomer
            );
    }
}