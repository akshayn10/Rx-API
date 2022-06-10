using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionStatsUseCase():IRequest<SubscriptionStatsVm>;

public class GetSubscriptionStatsUseCaseHandler : IRequestHandler<GetSubscriptionStatsUseCase, SubscriptionStatsVm>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetSubscriptionStatsUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public Task<SubscriptionStatsVm> Handle(GetSubscriptionStatsUseCase request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}