using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Primary.SystemSubscription;

public record CancelSystemSubscriptionUseCase(Guid SubscriptionId):IRequest<string>;
public class CancelSystemSubscriptionUseCaseHandler : IRequestHandler<CancelSystemSubscriptionUseCase, string>
{
    private readonly IPrimaryServiceManager _primaryServiceManager;

    public CancelSystemSubscriptionUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
    {
        _primaryServiceManager = primaryServiceManager;
    }
    public async Task<string> Handle(CancelSystemSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return await _primaryServiceManager.SystemSubscriptionService.CancelSubscription(request.SubscriptionId);
    }
}