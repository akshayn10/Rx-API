namespace Rx.Domain.DTOs.Tenant.Bill;

public record BillVm(string BillId,string? CustomerName,string? Email,string? GeneratedDate,decimal TotalAmount,List<SubscriptionForBill> SubscriptionsForBill);