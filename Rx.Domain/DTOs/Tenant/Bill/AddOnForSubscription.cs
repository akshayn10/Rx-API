namespace Rx.Domain.DTOs.Tenant.Bill;

public record AddOnForSubscription(string ConsumedDate,string AddOnName,int Units,decimal UnitPrice,decimal TotalAmount);