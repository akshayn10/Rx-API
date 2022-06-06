namespace Rx.Domain.DTOs.Tenant.Bill;

public record AddOnForSubscription(string date,string addOnName,int units,decimal unitPrice,decimal totalAmount);