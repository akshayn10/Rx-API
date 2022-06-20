using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Dashboard;
using Rx.Domain.DTOs.Tenant.Report;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Dashboard;

public record GetTableStatUseCase():IRequest<TableVm>;
public class GetTableStatUseCaseHandler : IRequestHandler<GetTableStatUseCase, TableVm>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetTableStatUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<TableVm> Handle(GetTableStatUseCase request, CancellationToken cancellationToken)
    {
        var topPlans =await  _tenantDbContext.Subscriptions!.GroupBy(s => s.ProductPlan!.Name)
            .OrderByDescending(x => x.Count()).Take(4).Select(
                x=>new TableStats(
                    x.First().ProductPlan.Product.Name,x.Key!
                    )
                ).ToListAsync(cancellationToken: cancellationToken);
        var topAddOns =await  _tenantDbContext.AddOnUsages!.GroupBy(a => a.AddOn!.Name)
            .OrderByDescending(x => x.Count()).Take(4).Select(
                x=>new TableStats(
                    x.First().AddOn!.Product.Name,x.Key!
                )
            ).ToListAsync(cancellationToken: cancellationToken);
        var tableVm = new TableVm(
            topPlans, topAddOns
        );
        return tableVm;
    }
}