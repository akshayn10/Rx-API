using Rx.Domain.DTOs.Tenant.Transaction;

namespace Rx.Domain.Interfaces.Tenant;

public interface ITransactionService
{
    Task<TransactionDto> AddTransaction(Guid BillId,TransactionForCreationDto transactionForCreationDto);

}