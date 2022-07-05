using System.Text.Json.Nodes;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rx.Domain.DTOs.Tenant.Bill;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Billing;

public record GetBillByIdUseCase(Guid BillId):IRequest<BillVm>;

public class GetBillByIdUseCaseHandler : IRequestHandler<GetBillByIdUseCase, BillVm>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetBillByIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<BillVm> Handle(GetBillByIdUseCase request, CancellationToken cancellationToken)
    {
        var bill =await _tenantDbContext.Bills.Include(b=>b.OrganizationCustomer).Select(
            b =>new
            {
                b.BillId,b.OrganizationCustomer.Name,b.OrganizationCustomer.Email,b.GeneratedDate,
                    b.TotalAmount,b.BillDetails
            }
            ).FirstOrDefaultAsync(b=>b.BillId==request.BillId, cancellationToken: cancellationToken);
        
        List<SubscriptionForBill> subscriptionForBill = null;

        if (bill.BillDetails != null)
        {
            subscriptionForBill =JsonConvert.DeserializeObject<List<SubscriptionForBill>>(bill.BillDetails);
        }
        var billVm = new BillVm(
            bill.BillId.ToString(),
            bill.Name,
            bill.Email,
            bill.GeneratedDate.ToShortDateString().ToString()+" "+bill.GeneratedDate.ToLongTimeString(),
            bill.TotalAmount,
            subscriptionForBill
        );
        return billVm;

    }
    
}