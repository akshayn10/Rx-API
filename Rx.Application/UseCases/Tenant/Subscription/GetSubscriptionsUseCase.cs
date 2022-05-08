using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionsUseCase():IRequest<IEnumerable<SubscriptionVm>>;

public class GetSubscriptionUseCaseHandler : IRequestHandler<GetSubscriptionsUseCase, IEnumerable<SubscriptionVm>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public GetSubscriptionUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<SubscriptionVm>> Handle(GetSubscriptionsUseCase request, CancellationToken cancellationToken)
    {
        var subscriptions = await (from s in _tenantDbContext.Subscriptions
                join oc in _tenantDbContext.OrganizationCustomers on s.OrganizationCustomerId equals oc.CustomerId
                join pp in _tenantDbContext.ProductPlans on s.ProductPlanId equals pp.PlanId
                join p in _tenantDbContext.Products on pp.ProductId equals p.ProductId
                select new
                {
                    s.SubscriptionId, CustomerName = oc.Name,ProductName=p.Name, PlanName = pp.Name, s.CreatedDate, s.EndDate, s.IsActive
                }).ToListAsync(cancellationToken);
        var subscriptionsVms = subscriptions.Select(s =>
            new SubscriptionVm(
                subscriptionId: s.SubscriptionId.ToString(),
                customerName: s.CustomerName,
                product: s.ProductName,
                plan: s.PlanName,
                createdDate: s.CreatedDate.ToString(),
                endDate: s.EndDate.ToString(),
                status: s.IsActive ? "Active" : "Inactive"
            )
        );
        return subscriptionsVms;
    }
}
// string subscriptionId, string customerName, string product, string plan, string createdDate, string endDate, string status 