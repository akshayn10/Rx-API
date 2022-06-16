using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;

namespace Rx.Application.UseCases.Tenant.Billing;

public record GetBillByCustomerIdUseCase(Guid CustomerId):IRequest<IEnumerable<BillDto>>;

public class GetBillsByCustomerIdUseCaseHandler : IRequestHandler<GetBillByCustomerIdUseCase, IEnumerable<BillDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;
    private readonly IBillingService _billingService;

    public GetBillsByCustomerIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper,IBillingService billingService)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
        _billingService = billingService;
    }
    public async Task<IEnumerable<BillDto>> Handle(GetBillByCustomerIdUseCase request, CancellationToken cancellationToken)
    {
        var bills = await _tenantDbContext.Bills.Include(b=>b.OrganizationCustomer).Where(b=>b.CustomerId==request.CustomerId).Select(
                b=>new BillDto(
                    b.BillId.ToString(),
                    b.GeneratedDate.ToString(),
                    b.TotalAmount,
                    b.OrganizationCustomer.Name
                )
            )
            .ToListAsync(cancellationToken: cancellationToken);
        
        return bills;
    }
}