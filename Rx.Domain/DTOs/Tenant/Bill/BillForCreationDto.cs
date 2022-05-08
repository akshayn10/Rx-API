namespace Rx.Domain.DTOs.Tenant.Bill;

public record BillForCreationDto(DateTime BillDate,decimal TotalAmount,Guid SubscriptionId);