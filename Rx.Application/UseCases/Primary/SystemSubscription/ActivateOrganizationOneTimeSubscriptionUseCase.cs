using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Primary.SystemSubscription;

public record ActivateOrganizationOneTimeSubscriptionUseCase(Guid SubscriptionId):IRequest<string>;

public class ActivateOrganizationOneTimeSubscriptionUseCaseHandler : IRequestHandler<ActivateOrganizationOneTimeSubscriptionUseCase, string>
{
    private readonly IPrimaryServiceManager _primaryServiceManager;

    public ActivateOrganizationOneTimeSubscriptionUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
    {
        _primaryServiceManager = primaryServiceManager;
    }
    public async Task<string> Handle(ActivateOrganizationOneTimeSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return await _primaryServiceManager.SystemSubscriptionService.ActivateOneTimeSubscription(request.SubscriptionId);
    }
}