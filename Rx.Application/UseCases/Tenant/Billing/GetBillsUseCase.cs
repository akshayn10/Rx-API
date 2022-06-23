

using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Billing;

public record GetBillsUseCase(string SearchKey):IRequest<IEnumerable<BillDto>>;

public class GetBillUseCaseHandler : IRequestHandler<GetBillsUseCase, IEnumerable<BillDto>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetBillUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<IEnumerable<BillDto>> Handle(GetBillsUseCase request, CancellationToken cancellationToken)
    {
        var bills = await _tenantDbContext.Bills.Include(b=>b.OrganizationCustomer).Select(
            b=>new BillDto(
                b.BillId.ToString(),
                b.GeneratedDate.ToString(),
                b.TotalAmount,
                b.OrganizationCustomer.Name
                )
            )
            .ToListAsync();
        
        return bills.Where(b=>b.CustomerName.ToLower().StartsWith(request.SearchKey));

    }
}

