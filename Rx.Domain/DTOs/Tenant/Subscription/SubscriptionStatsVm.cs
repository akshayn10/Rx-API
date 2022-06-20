namespace Rx.Domain.DTOs.Tenant.Subscription;

public record SubscriptionStatsVm(
    int TotalSubscriptions,
    int ActiveSubscriptions,
    int ActiveTrialSubscriptions,
    int UpgradeCount,
    int DowngradeCount,
    int ActiveOneTimeSubscriptions,
    int ActiveRecurringSubscriptions,
    int CancelledSubscriptions
    );