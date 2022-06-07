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

  
    

    public async Task<TransactionDto> AddTransaction(Guid subscriptionId,TransactionForCreationDto transactionForCreationDto)
    {
        var subscription = await _tenantDbContext.Subscriptions!.FirstOrDefaultAsync(x => x.SubscriptionId == subscriptionId);
        if (subscription == null)
        {
            throw new InvalidOperationException("Subscription not found");
        }
        
        var transaction = _mapper.Map<PaymentTransaction>(transactionForCreationDto);
        transaction.TransactionDate = DateTime.Now;
        await _tenantDbContext.PaymentTransactions.AddAsync(transaction);
        await _tenantDbContext.SaveChangesAsync();
        return _mapper.Map<TransactionDto>(transaction);
    }
}