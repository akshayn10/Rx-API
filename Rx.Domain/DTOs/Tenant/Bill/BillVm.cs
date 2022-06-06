namespace Rx.Domain.DTOs.Tenant.Bill;

public record BillVm(string billId,string? customerName,string? Email,string? GeneratedDate,decimal totalAmount);