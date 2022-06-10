using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Customer;
public record GetCustomersUseCase(string SearchKey):IRequest<IEnumerable<CustomerVm>>;

public class GetCustomersUseCaseHandler : IRequestHandler<GetCustomersUseCase, IEnumerable<CustomerVm>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetCustomersUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public  async Task<IEnumerable<CustomerVm>> Handle(GetCustomersUseCase request, CancellationToken cancellationToken)
    {
        var customerVmms = await _tenantDbContext.OrganizationCustomers!.Include(c => c.Subscriptions)
            .Select(c=>
            new CustomerVm(
                c.CustomerId.ToString(),
                c.Name,
                c.Email,
                c.Subscriptions!.Any(s=>s.IsActive)?"Active":"Inactive",
                c.Last4
            )
        ).ToListAsync(cancellationToken: cancellationToken);
        
        //comment
        // var customers = await (from c in _tenantDbContext.OrganizationCustomers
        //     join s in _tenantDbContext.Subscriptions on c.CustomerId equals s.OrganizationCustomerId
        //     select
        //         new
        //         {
        //             c.CustomerId,c.Email,c.Name,s.IsActive,c.Last4
        //         }
        //     ).OrderByDescending(c=>c.IsActive).ToListAsync(cancellationToken);
        //
        // var customerVm = customers.Select(
        //     c => new CustomerVm(
        //         customerId: c.CustomerId.ToString(),
        //         name: c.Name,
        //         email: c.Email,
        //         status: c.IsActive ? "Active" : "Inactive",
        //         last4:c.Last4
        //     )
        // ).Where(c=>c.name!.ToLower().StartsWith(request.SearchKey)).DistinctBy(x => x.customerId);
        
        
            return customerVmms.Where(c=>c.name.ToLower().StartsWith(request.SearchKey));
            
    }
}