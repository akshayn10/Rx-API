namespace Rx.Domain.DTOs.Tenant.Transaction;

public record TransactionForCreationDto(
    DateTime TransactionDate,
    decimal TransactionAmount,string TransactionDescription,
    string TransactionStatus, Guid SubscriptionId);