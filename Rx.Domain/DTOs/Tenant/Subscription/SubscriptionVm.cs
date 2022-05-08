namespace Rx.Domain.DTOs.Tenant.Subscription;

public record SubscriptionVm(string subscriptionId, string? customerName, string? product, string? plan, string? createdDate, string? endDate, string status);