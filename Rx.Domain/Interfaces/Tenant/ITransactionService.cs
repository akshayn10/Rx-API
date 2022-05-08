using Rx.Domain.DTOs.Tenant.Transaction;

namespace Rx.Domain.Interfaces.Tenant;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetTransactions();
    Task<TransactionDto> GetTransactionById(Guid transactionId);
    Task<TransactionDto> AddTransaction(TransactionForCreationDto transactionForCreationDto);

}