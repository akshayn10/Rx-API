using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Transaction;

public record GetTransactionDtosUseCase():IRequest<IEnumerable<TransactionDto>>;

public class GetTransactionDtosUseCaseHandler : IRequestHandler<GetTransactionsUseCase, IEnumerable<TransactionDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetTransactionDtosUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionsUseCase request, CancellationToken cancellationToken)
    {
        var transactions = await _tenantDbContext.PaymentTransactions.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }
}