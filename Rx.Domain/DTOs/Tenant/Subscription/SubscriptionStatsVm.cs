namespace Rx.Domain.DTOs.Tenant.Subscription;

public record SubscriptionStatsVm(int TotalSubscriptions,
    int ActiveSubscription,int ActiveTrialSubscriptions,
    int UpgradeCount,int DowngradeCount,
    int OneTimeSubscription,int RecurringSubscription);