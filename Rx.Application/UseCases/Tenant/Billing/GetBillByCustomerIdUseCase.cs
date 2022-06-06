using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Billing;

public record GetBillByCustomerIdUseCase(Guid CustomerId):IRequest<BillDetailsVm>;

public class GetBillsByCustomerIdUseCaseHandler : IRequestHandler<GetBillByCustomerIdUseCase, BillDetailsVm>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetBillsByCustomerIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<BillDetailsVm> Handle(GetBillByCustomerIdUseCase request, CancellationToken cancellationToken)
    {
        var bills = await (from b in _tenantDbContext.Bills
            join c in _tenantDbContext.OrganizationCustomers on b.CustomerId equals c.CustomerId
            select new {c.Name,c.Email,b.GeneratedDate,b.TotalAmount,b.BillId}).ToListAsync(cancellationToken);
        var bill = bills.First();
        var subscriptionsForCustomer =await _tenantDbContext.Subscriptions!.Where(s => s.OrganizationCustomerId == request.CustomerId).Include(a=>a.AddOnUsages)
            .Select(s=>
                new SubscriptionForBill(
                    s.CreatedDate.ToString(),
                    s.ProductPlan.Product.Name,
                    s.ProductPlan.Name,
                    s.ProductPlan.Price,
                    s.AddOnUsages.Select(a=>
                        new AddOnForSubscription(
                            a.Date.ToString(),
                            a.AddOn.Name,
                            a.Unit,
                            a.TotalAmount/a.Unit,
                            a.TotalAmount
                            )
                        )
                    )
                ).ToListAsync();
        var billDetailVm = new BillDetailsVm(
            createdDate: bill.GeneratedDate.ToString(),
            billId: bill.BillId.ToString(),
            customerName: bill.Name,
            subscriptionsForBill: subscriptionsForCustomer
        );
        return billDetailVm;

    }
}