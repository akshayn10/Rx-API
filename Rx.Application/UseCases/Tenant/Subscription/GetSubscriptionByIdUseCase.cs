using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionByIdUseCase(Guid SubscriptionId):IRequest<SubscriptionVm>;

public class GetSubscriptionByIdUseCaseHandler : IRequestHandler<GetSubscriptionByIdUseCase,SubscriptionVm>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetSubscriptionByIdUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<SubscriptionVm> Handle(GetSubscriptionByIdUseCase request, CancellationToken cancellationToken)
    {
        var subscription =await _tenantDbContext.Subscriptions!.FindAsync(request.SubscriptionId);
        var plan = await _tenantDbContext.ProductPlans!.FirstOrDefaultAsync(p=>p.PlanId==subscription!.ProductPlanId, cancellationToken: cancellationToken);
        var product = await _tenantDbContext.Products!.FirstOrDefaultAsync(p=>p.ProductId==plan!.ProductId, cancellationToken: cancellationToken);
        var customer = await _tenantDbContext.OrganizationCustomers!.FindAsync(subscription!.OrganizationCustomerId);
        return new SubscriptionVm(
            subscription!.SubscriptionId.ToString(),
            customer!.Name,
            product!.Name,
            plan!.Name,
            subscription.CreatedDate.ToString(),
            subscription.EndDate.ToString(),
            subscription.IsActive ? "Active" : "Inactive",
            subscriptionType: subscription.SubscriptionType ? "Recurring" : "One Time",
            subscription.IsTrial
            );
    }
}