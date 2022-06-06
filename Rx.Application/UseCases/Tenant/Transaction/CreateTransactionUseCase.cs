using MediatR;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Transaction;

public record CreateTransactionUseCase(Guid SubscriptionId,TransactionForCreationDto TransactionForCreationDto):IRequest<TransactionDto>;

public class CreateTransactionUseCaseHandler : IRequestHandler<CreateTransactionUseCase, TransactionDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public CreateTransactionUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public  Task<TransactionDto> Handle(CreateTransactionUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.TransactionService.AddTransaction(request.SubscriptionId, request.TransactionForCreationDto);
    }
}