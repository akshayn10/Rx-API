namespace Rx.Domain.DTOs.Tenant.Bill;

public record SubscriptionForBill(string SubscriptionId,string date,string productName,string planName,decimal price,IEnumerable<AddOnForSubscription> addOnsForSubscription);