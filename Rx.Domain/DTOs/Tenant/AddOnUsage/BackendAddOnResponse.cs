namespace Rx.Domain.DTOs.Tenant.AddOnUsage;

public record BackendAddOnResponse(string EventType,string CustomerId,string SubscriptionId,string AddOnId);