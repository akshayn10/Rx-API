namespace Rx.Domain.DTOs.Tenant.Bill;

public record BillVm(string? Name,string? Email,string? GeneratedDate,decimal Amount);