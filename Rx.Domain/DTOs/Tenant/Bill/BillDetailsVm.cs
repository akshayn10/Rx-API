namespace Rx.Domain.DTOs.Tenant.Bill;

public record BillDetailsVm(string createdDate,string billId,string customerName,List<SubscriptionForBill> subscriptionsForBill);