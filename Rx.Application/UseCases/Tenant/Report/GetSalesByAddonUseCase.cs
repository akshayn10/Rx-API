namespace Rx.Application.UseCases.Tenant.Report;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Report;
using Rx.Domain.Interfaces.DbContext;


public record GetSalesByAddonUseCase():IRequest<IEnumerable<Stats>>;

public class GetSalesByAddonUseCaseHandler : IRequestHandler<GetSalesByAddonUseCase, IEnumerable<Stats>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetSalesByAddonUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<IEnumerable<Stats>> Handle(GetSalesByAddonUseCase request, CancellationToken cancellationToken)
    {
        var result =await _tenantDbContext.AddOnUsages!
            .Include(s=>s.AddOn)
            .Include(s=>s.AddOn.Product)
            .Select(x=>
                new
                {
                    pName = x.AddOn.Name,x.AddOnId,ppName=x.AddOn.Product.Name,x.AddOn
                }
            )
            .ToListAsync(cancellationToken: cancellationToken);
        var statCount = result.GroupBy(s => s.AddOn).Select(x => new Stats(
                x.Key.Product.Name + "/" + x.Key.Name,
                x.Count()
            )
        ).ToList();
       
        return statCount;
    }
}