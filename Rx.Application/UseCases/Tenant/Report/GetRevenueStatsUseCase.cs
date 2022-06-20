using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Report;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Report;

public record GetRevenueStatsUseCase():IRequest<IEnumerable<Stats>>;

public class GetRevenueStatsUseCaseHandler : IRequestHandler<GetRevenueStatsUseCase, IEnumerable<Stats>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetRevenueStatsUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<IEnumerable<Stats>> Handle(GetRevenueStatsUseCase request, CancellationToken cancellationToken)
    {
        var monthsRequired = 6;
        var result =await _tenantDbContext.PaymentTransactions!
            .Where(p=>p.TransactionStatus == "success")
            .ToListAsync(cancellationToken: cancellationToken);
        var statCount = result.GroupBy(x => x.TransactionDate.ToString("MMMM")).Select(x => new Stats(
                x.Key,
                Convert.ToInt32(x.Sum(a=>a.TransactionAmount))
            )
        ).ToList(); 
        var stats =new  Stats[monthsRequired];
        for (var i = 0; i < monthsRequired; i++)
        {
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
