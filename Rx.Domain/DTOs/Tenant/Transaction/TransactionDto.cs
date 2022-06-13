namespace Rx.Domain.DTOs.Tenant.Transaction;

public record TransactionDto(Guid TransactionId, DateTime TransactionDate,
    decimal TransactionAmount, string TransactionDescription, string TransactionStatus, Guid SubscriptionId);

