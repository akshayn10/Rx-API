using MediatR;
using Rx.Domain.DTOs.Tenant.Transaction;

namespace Rx.Application.UseCases.Tenant.Transaction;

public record GetTransactionsUseCase():IRequest<IEnumerable<TransactionDto>>;

public class GetTransactionsUseCaseHandler : IRequestHandler<GetTransactionsUseCase, IEnumerable<TransactionDto>>
{
    public Task<IEnumerable<TransactionDto>> Handle(GetTransactionsUseCase request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}