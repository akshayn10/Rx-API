namespace Rx.Domain.DTOs.Tenant.Transaction;

public record TransactionDto(Guid TransactionId, DateTime TransactionDate,
    decimal TransactionAmount, string TransactionDescription,
    string TransactionPaymentReferenceId, string TransactionPaymentGatewayResponse, string TransactionStatus,
    string TransactionCurrency, Guid SubscriptionId);

