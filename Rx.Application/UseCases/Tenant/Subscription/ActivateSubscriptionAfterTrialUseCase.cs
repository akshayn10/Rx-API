using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record ActivateSubscriptionAfterTrialUseCase(Guid SubscriptionId):IRequest<string>;

public class ActivateSubscriptionAfterTrialUseCaseHandler : IRequestHandler<ActivateSubscriptionAfterTrialUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public ActivateSubscriptionAfterTrialUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }

    public async Task<string> Handle(ActivateSubscriptionAfterTrialUseCase request, CancellationToken cancellationToken)
    {

        return await _tenantServiceManager.SubscriptionService.ActivateSubscriptionAfterTrial(request.SubscriptionId);
    }



}
