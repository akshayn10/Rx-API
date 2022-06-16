using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Primary.SystemSubscription;

public record ActivateOrganizationRecurringSubscriptionUseCase(Guid SubscriptionId):IRequest<string>;

public class
    ActivateOrganizationRecurringSubscriptionUseCaseHandler : IRequestHandler<ActivateOrganizationRecurringSubscriptionUseCase, string>
{
    private readonly IPrimaryServiceManager _primaryServiceManager;

    public ActivateOrganizationRecurringSubscriptionUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
    {
        _primaryServiceManager = primaryServiceManager;
    }
    public async Task<string> Handle(ActivateOrganizationRecurringSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return await _primaryServiceManager.SystemSubscriptionService.ActivateRecurringSubscription(request.SubscriptionId);
    }
}