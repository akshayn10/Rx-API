using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Customer;

public record GetCustomerByIdUseCase(Guid CustomerId):IRequest<CustomerVm>;

public class GetCustomerByIdUseCaseHandler : IRequestHandler<GetCustomerByIdUseCase,CustomerVm>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetCustomerByIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<CustomerVm> Handle(GetCustomerByIdUseCase request, CancellationToken cancellationToken)
    {
        var customer = await _tenantDbContext.OrganizationCustomers!.FindAsync(request.CustomerId);
        var subscriptions = await _tenantDbContext.Subscriptions!.Where(s=>s.OrganizationCustomerId==request.CustomerId).OrderByDescending(s=>s.IsActive).ToListAsync(cancellationToken);
        var subscription = subscriptions.First();
        var customerVm = new CustomerVm(customer?.CustomerId.ToString(), customer?.Name,
            email: customer?.Email,
            status: subscription.IsActive ? "Active" : "Inactive"
        );
        return customerVm;
    }
}