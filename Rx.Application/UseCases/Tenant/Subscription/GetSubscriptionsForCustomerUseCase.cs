using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionsForCustomerUseCase(Guid CustomerId):IRequest<IEnumerable<CustomerSubscriptionsVm>>;

public class GetSubscriptionsForCustomerUseCaseHandler : IRequestHandler<GetSubscriptionsForCustomerUseCase, IEnumerable<CustomerSubscriptionsVm>>
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;
    public GetSubscriptionsForCustomerUseCaseHandler(ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CustomerSubscriptionsVm>> Handle(GetSubscriptionsForCustomerUseCase request, CancellationToken cancellationToken)
    {

                

                var subscriptions = await (from s in _tenantDbContext.Subscriptions
                        join oc in _tenantDbContext.OrganizationCustomers on s.OrganizationCustomerId equals oc.CustomerId
                        join pp in _tenantDbContext.ProductPlans on s.ProductPlanId equals pp.PlanId
                        join p in _tenantDbContext.Products on pp.ProductId equals p.ProductId
                        where oc.CustomerId == request.CustomerId
                        select new
                        {
                            s.SubscriptionId, s.CreatedDate, s.EndDate, p.Name, planName = pp.Name, s.IsActive
                        }
            
                    ).ToListAsync(cancellationToken);
                var subscriptionVms = subscriptions.Select(
                    s =>
                        new CustomerSubscriptionsVm(
                            subscriptionId: s.SubscriptionId.ToString(),
                            product: s.Name,
                            plan: s.planName,
                            createdDate: s.CreatedDate.ToString(),
                            endDate: s.EndDate.ToString(),
                            status: s.IsActive ? "Active" : "Inactive"
                        )
                );
                return subscriptionVms;
    }
}