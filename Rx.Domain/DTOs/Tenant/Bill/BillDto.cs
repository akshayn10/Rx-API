namespace Rx.Domain.DTOs.Tenant.Bill;

public record BillDto(Guid BillId,DateTime GeneratedDate,decimal TotalAmount,Guid CustomerId);

