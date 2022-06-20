using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Report;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Dashboard;

public record GetRevenueStatUseCase():IRequest<IEnumerable<Stats>>;
public class GetRevenueStatUseCaseHandler : IRequestHandler<GetRevenueStatUseCase, IEnumerable<Stats>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetRevenueStatUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<IEnumerable<Stats>> Handle(GetRevenueStatUseCase request, CancellationToken cancellationToken)
    {
        var monthsRequired = 6;
        var result =await _tenantDbContext.PaymentTransactions!
            .Where(p=>p.TransactionDate>DateTime.Now.AddMonths(-1*monthsRequired))
            .ToListAsync(cancellationToken: cancellationToken);
        var statCount = result.GroupBy(x => x.TransactionDate.ToString("MMMM")).Select(x => new Stats(
                x.Key,
                Convert.ToInt32(x.Sum(p=>p.TransactionAmount))
                  
            )
        ).ToList();
        var stats =new  Stats[monthsRequired];
        for (var i = 0; i < monthsRequired; i++)
        {
            // var month = DateTimeFormatInfo.CurrentInfo.GetMonthName(i+1);
            var month =DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Now.AddMonths(i-(monthsRequired-1)).Month) ;
            var index = statCount.FindIndex(x => x.Type == month);
            var count = 0;
            if (index != -1)
            {
                count = statCount[index].Count;
            }
            stats[i] = new Stats(month, count);
        }
        return stats;
    }
}