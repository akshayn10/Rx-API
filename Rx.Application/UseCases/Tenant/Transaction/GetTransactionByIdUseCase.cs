using MediatR;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Transaction;

public record GetTransactionByIdUseCase(Guid TransactionId):IRequest<TransactionDto>;

public class GetTransactionByIdUseCaseHandler : IRequestHandler<GetTransactionByIdUseCase, TransactionDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetTransactionByIdUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<TransactionDto> Handle(GetTransactionByIdUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.TransactionService.GetTransactionById(request.TransactionId);
    }
}
