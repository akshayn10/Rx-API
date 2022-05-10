using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Entities.Tenant;
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

  
    

    public async Task<TransactionDto> AddTransaction(Guid billId,TransactionForCreationDto transactionForCreationDto)
    {
        var bill = await _tenantDbContext.Bills.FirstOrDefaultAsync(x => x.BillId == billId);
        if (bill == null)
        {
            throw new InvalidOperationException("Bill not found");
        }
        
        var transaction = _mapper.Map<PaymentTransaction>(transactionForCreationDto);
        await _tenantDbContext.PaymentTransactions.AddAsync(transaction);
        await _tenantDbContext.SaveChangesAsync();
        return _mapper.Map<TransactionDto>(transaction);
    }
}