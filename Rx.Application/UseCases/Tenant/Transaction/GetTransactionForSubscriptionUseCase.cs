using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Transaction;

public record GetTransactionForSubscriptionUseCase(Guid SubscriptionId):IRequest<IEnumerable<TransactionDto>>;

public class GetTransactionForSubscriptionUseCaseHandler: IRequestHandler<GetTransactionForSubscriptionUseCase,IEnumerable<TransactionDto>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetTransactionForSubscriptionUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionForSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        var transactions =await _tenantDbContext.PaymentTransactions.Where(x=>x.SubscriptionId==request.SubscriptionId).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }
}