namespace Rx.Domain.DTOs.Tenant.Transaction;

public record TransactionVm(string TransactionId,string Date,string SubscriptionId,string ProductName,string CustomerName,decimal Amount,string Status,string PaymentFor,string AddonName);