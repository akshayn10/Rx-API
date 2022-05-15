using AutoMapper;
using MediatR;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Customer;

public record GetCustomerByIdUseCase(Guid Id):IRequest<OrganizationCustomerDto>;

public class GetCustomerByIdUseCaseHandler : IRequestHandler<GetCustomerByIdUseCase,OrganizationCustomerDto>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetCustomerByIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<OrganizationCustomerDto> Handle(GetCustomerByIdUseCase request, CancellationToken cancellationToken)
    {
        var customer = await _tenantDbContext.OrganizationCustomers!.FindAsync(request.Id);
        return _mapper.Map<OrganizationCustomerDto>(customer);
    }
}