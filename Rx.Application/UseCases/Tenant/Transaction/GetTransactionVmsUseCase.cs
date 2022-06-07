using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Transaction;

public record GetTransactionVmsUseCase():IRequest<IEnumerable<TransactionVm>>;

public class GetTransactionVmsUseCaseHandler : IRequestHandler<GetTransactionVmsUseCase, IEnumerable<TransactionVm>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetTransactionVmsUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task<IEnumerable<TransactionVm>> Handle(GetTransactionVmsUseCase request, CancellationToken cancellationToken)
    {
        var transactions = await (from t in _tenantDbContext.PaymentTransactions
            join s in _tenantDbContext.Subscriptions on t.SubscriptionId equals s.SubscriptionId
            join c in _tenantDbContext.OrganizationCustomers on s.OrganizationCustomerId equals c.CustomerId
            join pp in _tenantDbContext.ProductPlans on s.ProductPlanId equals pp.PlanId
            join p in _tenantDbContext.Products on pp.ProductId equals p.ProductId
            select new
            {
                t.TransactionId, t.TransactionDate, t.SubscriptionId, productName = p.Name, customerName = c.Name,
                t.TransactionAmount, t.TransactionStatus, t.TransactionDescription
            }).ToListAsync(cancellationToken: cancellationToken);
        var transactionVms = transactions.Select(t=>
            new TransactionVm(
                TransactionId:t.TransactionId.ToString(),
                Date:t.TransactionDate.ToString(),
                SubscriptionId:t.SubscriptionId.ToString(),
                ProductName:t.productName,
                CustomerName:t.customerName,
                Amount:t.TransactionAmount,
                Status:t.TransactionStatus,
                PaymentFor:t.TransactionDescription==""?"Subscription":"Add on",
                AddonName:t.TransactionDescription
            )
            );
        return transactionVms;
    }
}