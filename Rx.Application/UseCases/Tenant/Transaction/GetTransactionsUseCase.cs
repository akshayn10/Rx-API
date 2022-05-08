using MediatR;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.Transaction;

public record GetTransactionsUseCase():IRequest<IEnumerable<TransactionDto>>;

public class GetTransactionsUseCaseHandler : IRequestHandler<GetTransactionsUseCase, IEnumerable<TransactionDto>>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetTransactionsUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }

    public Task<IEnumerable<TransactionDto>> Handle(GetTransactionsUseCase request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}