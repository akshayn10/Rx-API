namespace Rx.Domain.DTOs.Tenant.Bill;

public record BillDto(string BillId,string GeneratedDate,decimal TotalAmount,string CustomerName);

