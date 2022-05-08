using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Transaction;

public record GetTransactionsByBillIdUseCase(Guid BillId):IRequest<IEnumerable<TransactionDto>>;

public class
    GetTransactionsByBillIdUseCaseHandler : IRequestHandler<GetTransactionsByBillIdUseCase, IEnumerable<TransactionDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetTransactionsByBillIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionsByBillIdUseCase request, CancellationToken cancellationToken)
    {
        var transactions = await _tenantDbContext.PaymentTransactions.Where(t=>t.BillId==request.BillId).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }
}