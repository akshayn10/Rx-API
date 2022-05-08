namespace Rx.Domain.DTOs.Tenant.Transaction;

public record TransactionForCreationDto(DateTime TransactionDate,
    decimal TransactionAmount,string TransactionDescription,
    string TransactionPaymentReferenceId,string TransactionPaymentGatewayResponse, string TransactionStatus,
    string TransactionCurrency, Guid BillId);