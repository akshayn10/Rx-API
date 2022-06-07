using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.AddOnUsage;

public record GetAddOnUsagesForSubscriptionUseCase(Guid SubscriptionId):IRequest<IEnumerable<AddOnUsageVm>>;

public class
    GetAddOnUsagesForSubscriptionUseCaseHandler : IRequestHandler<GetAddOnUsagesForSubscriptionUseCase,IEnumerable<AddOnUsageVm>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetAddOnUsagesForSubscriptionUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<IEnumerable<AddOnUsageVm>> Handle(GetAddOnUsagesForSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        var usages = await (from aou in _tenantDbContext.AddOnUsages.Where(x => x.SubscriptionId == request.SubscriptionId)
                join s in _tenantDbContext.Subscriptions on aou.SubscriptionId equals s.SubscriptionId
                join ao in _tenantDbContext.AddOns on aou.AddOnId equals ao.AddOnId
                from aop in _tenantDbContext.AddOnPricePerPlans
                where s.ProductPlanId == aop.ProductPlanId && aou.AddOnId == aop.AddOnId
                select new
                {
                    aou.Date,ao.Name,aou.Unit,aop.Price
                }).OrderByDescending(ao=>ao.Date).ToListAsync(cancellationToken: cancellationToken);
        var usageVms = usages.Select(x =>
            new AddOnUsageVm(x.Date.ToString(), x.Name, x.Unit, x.Unit * x.Price));
        return usageVms;
    }
}