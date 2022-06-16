namespace Rx.Domain.DTOs.Tenant.Bill;

public record SubscriptionForBill(string SubscriptionId,string StartDate,string ProductName,string PlanName,string SubscriptionType,decimal Price,IEnumerable<AddOnForSubscription> AddOnsForSubscription);