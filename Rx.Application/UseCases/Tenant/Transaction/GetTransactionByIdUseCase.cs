using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Transaction;

public record GetTransactionByIdUseCase(Guid TransactionId):IRequest<TransactionDto>;

public class GetTransactionByIdUseCaseHandler : IRequestHandler<GetTransactionByIdUseCase, TransactionDto>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetTransactionByIdUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<TransactionDto> Handle(GetTransactionByIdUseCase request, CancellationToken cancellationToken)
    {
        var transaction =
            await _tenantDbContext.PaymentTransactions.FirstOrDefaultAsync(t => t.TransactionId == request.TransactionId, cancellationToken: cancellationToken);

        return _mapper.Map<TransactionDto>(transaction);

    }
}
