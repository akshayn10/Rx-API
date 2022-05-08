using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Transaction;

public record GetTransactionsByCustomerIdUseCase(Guid CustomerId):IRequest<IEnumerable<TransactionDto>>;

public class GetTransactionsByCustomerIdUseCaseHandler : IRequestHandler<GetTransactionsByCustomerIdUseCase,IEnumerable<TransactionDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetTransactionsByCustomerIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }


    public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionsByCustomerIdUseCase request, CancellationToken cancellationToken)
    {
        var transactions =await (from t in _tenantDbContext.PaymentTransactions
            join b in _tenantDbContext.Bills on t.BillId equals b.BillId
            join s in _tenantDbContext.Subscriptions on b.SubscriptionId equals s.SubscriptionId
            where s.OrganizationCustomerId == request.CustomerId
            select t).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        
    }
}