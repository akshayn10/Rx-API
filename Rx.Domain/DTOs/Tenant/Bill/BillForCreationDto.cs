namespace Rx.Domain.DTOs.Tenant.Bill;

public record BillForCreationDto(DateTime GeneratedDate,decimal TotalAmount,Guid CustomerId,string BillDetails);