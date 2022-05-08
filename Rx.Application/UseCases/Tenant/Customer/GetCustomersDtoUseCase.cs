using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Customer;

public record GetCustomersDtoUseCase():IRequest<IEnumerable<OrganizationCustomerDto>>;

public class GetCustomersDtoUseCaseHandler : IRequestHandler<GetCustomersDtoUseCase, IEnumerable<OrganizationCustomerDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetCustomersDtoUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<OrganizationCustomerDto>> Handle(GetCustomersDtoUseCase request, CancellationToken cancellationToken)
    {
        var customers = await _tenantDbContext.OrganizationCustomers!.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<OrganizationCustomerDto>>(customers); ;
    }
}