namespace Rx.Domain.DTOs.Tenant.Bill;

public record SubscriptionForBill(string date,string productName,string planName,decimal price,IEnumerable<AddOnForSubscription> addOnsForSubscription);