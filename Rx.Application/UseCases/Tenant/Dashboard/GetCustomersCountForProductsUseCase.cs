using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Report;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Dashboard;

public record GetCustomersCountForProductsUseCase():IRequest<IEnumerable<Stats>>;

public class GetCustomersCountForProductsUseCaseHandler : IRequestHandler<GetCustomersCountForProductsUseCase,IEnumerable<Stats>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetCustomersCountForProductsUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<IEnumerable<Stats>> Handle(GetCustomersCountForProductsUseCase request, CancellationToken cancellationToken)
    {
        var result = await _tenantDbContext.Subscriptions!.GroupBy(x => x.ProductPlan!.Product!.Name).Select(x => new Stats(
                x.Key!,
                x.Count()
                )
        ).ToListAsync(cancellationToken: cancellationToken);
        return result;
    }
}