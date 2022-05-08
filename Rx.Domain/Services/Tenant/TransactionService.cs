using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Tenant;

namespace Rx.Domain.Services.Tenant;

public class TransactionService:ITransactionService
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TenantServiceManager> _logger;

    public TransactionService(ITenantDbContext tenantDbContext, IMapper mapper, ILogger<TenantServiceManager> logger)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactions()
    {
        var transactions = await _tenantDbContext.PaymentTransactions.ToListAsync();
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<TransactionDto> GetTransactionById(Guid transactionId)
    {
        var transaction = await _tenantDbContext.PaymentTransactions.FirstOrDefaultAsync(x => x.TransactionId == transactionId);
        return _mapper.Map<TransactionDto>(transaction);
        
    }

    public Task<TransactionDto> AddTransaction(TransactionForCreationDto transactionForCreationDto)
    {
        throw new NotImplementedException();
    }
}