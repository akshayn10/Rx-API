using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Report;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Report;

public record GetSalesByPlanUseCase():IRequest<IEnumerable<Stats>>;

public class GetSalesByPlanUseCaseHandler : IRequestHandler<GetSalesByPlanUseCase, IEnumerable<Stats>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetSalesByPlanUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<IEnumerable<Stats>> Handle(GetSalesByPlanUseCase request, CancellationToken cancellationToken)
    {
        var result =await _tenantDbContext.Subscriptions!
            .Include(s=>s.ProductPlan)
            .Include(s=>s.ProductPlan.Product)
            .Select(x=>
                new
                {
                    pName = x.ProductPlan.Name,x.ProductPlanId,ppName=x.ProductPlan.Product.Name,x.ProductPlan
                }
            )
            .ToListAsync(cancellationToken: cancellationToken);
        var statCount = result.GroupBy(s => s.ProductPlan).Select(x => new Stats(
                x.Key.Product.Name + "/" + x.Key.Name,
                x.Count()
            )
        ).ToList();
       
        return statCount;
    }
}