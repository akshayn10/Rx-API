namespace Rx.Domain.DTOs.Tenant.Bill;

public record BillDto(Guid BillId,DateTime BillDate,decimal TotalAmount,Guid SubscriptionId);

