using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record GetSubscriptionByIdForCustomerUseCase(Guid SubscriptionId):IRequest<SubscriptionVm>;

public class GetSubscriptionByIdForCustomerUseCaseHandler:IRequestHandler<GetSubscriptionByIdForCustomerUseCase,SubscriptionVm>{
    private readonly ITenantDbContext _tenantDbContext;

    public GetSubscriptionByIdForCustomerUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }


    public Task<SubscriptionVm> Handle(GetSubscriptionByIdForCustomerUseCase request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}