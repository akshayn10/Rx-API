namespace Rx.Domain.DTOs.Tenant.Subscription;

public record SubscriptionVm(string SubscriptionId, string? CustomerName, string? Product, string? Plan, string? CreatedDate, string? EndDate, string Status,string subscriptionType,bool isTrial);