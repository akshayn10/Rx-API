using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Dashboard;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Dashboard;

public record GetDashboardStatUseCase():IRequest<DashboardStats>;

public class GetDashboardStatUseCaseHandler : IRequestHandler<GetDashboardStatUseCase, DashboardStats>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetDashboardStatUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<DashboardStats> Handle(GetDashboardStatUseCase request, CancellationToken cancellationToken)
    {
        var totalCustomer = _tenantDbContext.OrganizationCustomers!.Count();
        var totalProducts =  _tenantDbContext.Products!.Count();
        var totalSubscription = _tenantDbContext.Subscriptions!.Count(s => !s.IsCancelled && (s.IsActive||s.IsTrial));
        var totalRevenue =await _tenantDbContext.PaymentTransactions.Where(p=>p.TransactionStatus=="Succeeded")
            .SumAsync(p => p.TransactionAmount, cancellationToken: cancellationToken);

        return new DashboardStats(totalCustomer, totalRevenue, totalSubscription, totalProducts);
    }
}