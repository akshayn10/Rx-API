using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Billing;

public record GetBillsByCustomerIdUseCase(Guid CustomerId):IRequest<IEnumerable<BillDto>>;

public class GetBillsByCustomerIdUseCaseHandler : IRequestHandler<GetBillsByCustomerIdUseCase, IEnumerable<BillDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetBillsByCustomerIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<BillDto>> Handle(GetBillsByCustomerIdUseCase request, CancellationToken cancellationToken)
    {
        var bills = await _tenantDbContext.Bills.Where(x => x.CustomerId == request.CustomerId).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<BillDto>>(bills);

    }
}