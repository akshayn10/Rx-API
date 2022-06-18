using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Report;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Report;

public record GetSubscriptionStatUseCase():IRequest<IEnumerable<Stats>>;

public class GetSubscriptionStatUseCaseHandler: IRequestHandler<GetSubscriptionStatUseCase, IEnumerable<Stats>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetSubscriptionStatUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }

    public async Task<IEnumerable<Stats>> Handle(GetSubscriptionStatUseCase request, CancellationToken cancellationToken)
    {
        var result =await _tenantDbContext.Subscriptions!.ToListAsync(cancellationToken: cancellationToken);
        var statCount = result.GroupBy(x => x.CreatedDate.ToString("MMMM")).Select(x => new Stats(
                x.Key,
                x.Count()
            )
        ).ToList();
        var stats =new  Stats[12];
        for (var i = 0; i < 12; i++)
        {
            var month = DateTimeFormatInfo.CurrentInfo.GetMonthName(i+1);
            var count = 0;
            foreach (var a in statCount.Where(a => a.Period.Equals(month)))
            {
                count = a.Count;
            }
            stats[i] = new Stats(month, count);
        }
        return stats;
    }
}