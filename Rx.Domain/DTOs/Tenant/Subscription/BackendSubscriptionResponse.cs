namespace Rx.Domain.DTOs.Tenant.Subscription;

public record BackendSubscriptionResponse(string EventType,string SubscriptionId,string CustomerId,string PlanId);