using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Product;

public record GetCustomersForProductUseCase(Guid ProductId):IRequest<IEnumerable<OrganizationCustomerDto>>;

public class
    GetCustomersForProductUseCaseHandler : IRequestHandler<GetCustomersForProductUseCase,IEnumerable<OrganizationCustomerDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetCustomersForProductUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<OrganizationCustomerDto>> Handle(GetCustomersForProductUseCase request, CancellationToken cancellationToken)
    {
        var customers = await (from s in _tenantDbContext.Subscriptions
            join c in _tenantDbContext.OrganizationCustomers on s.OrganizationCustomerId equals c.CustomerId
            join pp in _tenantDbContext.ProductPlans on s.ProductPlanId equals pp.PlanId
            join p in _tenantDbContext.Products on pp.ProductId equals p.ProductId
            where p.ProductId == request.ProductId
            select c).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<OrganizationCustomerDto>>(customers);
    }
}