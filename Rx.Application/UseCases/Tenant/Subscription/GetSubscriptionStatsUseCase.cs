using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionStatsUseCase():IRequest<SubscriptionStatsVm>;

public class GetSubscriptionStatsUseCaseHandler : IRequestHandler<GetSubscriptionStatsUseCase, SubscriptionStatsVm>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetSubscriptionStatsUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<SubscriptionStatsVm> Handle(GetSubscriptionStatsUseCase request, CancellationToken cancellationToken)
    {
        var totalSubscriptions =await  _tenantDbContext.Subscriptions!
            .CountAsync(s => !s.IsCancelled, cancellationToken: cancellationToken);
        var activeSubscriptions=await  _tenantDbContext.Subscriptions!
            .CountAsync(s =>  s.IsActive, cancellationToken: cancellationToken);
        var activeTrialSubscription =await _tenantDbContext.Subscriptions!
            .CountAsync(s =>  s.IsTrial, cancellationToken: cancellationToken);
        var downgradeCount =await _tenantDbContext.SubscriptionStats!
            .CountAsync(s => s.Change == "downgrade", cancellationToken: cancellationToken);
        var upgradeCount =await _tenantDbContext.SubscriptionStats!
            .CountAsync(s => s.Change == "upgrade", cancellationToken: cancellationToken);
        var activeOneTimeSubscription = await _tenantDbContext.Subscriptions!
            .CountAsync(s =>  s.IsActive&&!s.SubscriptionType, cancellationToken: cancellationToken);
        var activeRecurringSubscription =await _tenantDbContext.Subscriptions!
            .CountAsync(s =>  s.IsActive&&s.SubscriptionType, cancellationToken: cancellationToken);
        var cancelledSubscription = await _tenantDbContext.Subscriptions!
            .CountAsync(s =>  s.IsCancelled, cancellationToken: cancellationToken);

        return new SubscriptionStatsVm(
            totalSubscriptions,
            activeSubscriptions,
            activeTrialSubscription,
            upgradeCount,
            downgradeCount,
            activeOneTimeSubscription,
            activeRecurringSubscription,
            cancelledSubscription
        );
    }
}