

using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Billing;

public record GetBillsUseCase():IRequest<IEnumerable<BillVm>>;

public class GetBillUseCaseHandler : IRequestHandler<GetBillsUseCase, IEnumerable<BillVm>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetBillUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<IEnumerable<BillVm>> Handle(GetBillsUseCase request, CancellationToken cancellationToken)
    {
        var bills = await (from b in _tenantDbContext.Bills
            join c in _tenantDbContext.OrganizationCustomers on b.CustomerId equals c.CustomerId
            select new {c.Name,c.Email,b.GeneratedDate,b.TotalAmount,b.BillId}).ToListAsync(cancellationToken);
        var billVms = bills.Select(x=>new BillVm(
            billId:x.BillId.ToString(),
          customerName:x.Name,
            Email:x.Email,
            GeneratedDate:x.GeneratedDate.ToString(),
            totalAmount:x.TotalAmount
            ));
        return billVms;


    }
}

