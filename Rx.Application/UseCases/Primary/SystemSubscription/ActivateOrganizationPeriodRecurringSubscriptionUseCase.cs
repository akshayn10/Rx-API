using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Primary.SystemSubscription;

public record ActivateOrganizationPeriodRecurringSubscriptionUseCase(Guid SubscriptionId):IRequest<string>;

public class ActivateOrganizationPeriodRecurringSubscriptionUseCaseHandler:IRequestHandler<ActivateOrganizationPeriodRecurringSubscriptionUseCase,string>
{
    private readonly IPrimaryServiceManager _primaryServiceManager;

    public ActivateOrganizationPeriodRecurringSubscriptionUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
    {
        _primaryServiceManager = primaryServiceManager;
    }

    public async Task<string> Handle(ActivateOrganizationPeriodRecurringSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return await _primaryServiceManager.SystemSubscriptionService.ActivatePeriodRecurringSubscription(
            request.SubscriptionId);
    }
}