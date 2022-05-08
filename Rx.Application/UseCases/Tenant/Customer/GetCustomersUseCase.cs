using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Customer;
public record GetCustomersUseCase():IRequest<IEnumerable<CustomerVm>>;

public class GetCustomersUseCaseHandler : IRequestHandler<GetCustomersUseCase, IEnumerable<CustomerVm>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetCustomersUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public  async Task<IEnumerable<CustomerVm>> Handle(GetCustomersUseCase request, CancellationToken cancellationToken)
    {
        //comment
        var customers = await (from c in _tenantDbContext.OrganizationCustomers
            join s in _tenantDbContext.Subscriptions on c.CustomerId equals s.OrganizationCustomerId
            select
                new
                {
                    c.CustomerId,c.Email,c.Name,s.IsActive
                }
            ).OrderByDescending(c=>c.IsActive).ToListAsync(cancellationToken);
        
        var customersVm = customers.Select(
            c => new CustomerVm(
                customerId: c.CustomerId.ToString(),
                name: c.Name,
                email: c.Email,
                status: c.IsActive ? "Active" : "Inactive"
            )
        ).DistinctBy(x => x.customerId);

        return customersVm;

    }
}