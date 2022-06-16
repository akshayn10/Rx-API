using MediatR;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Primary.SystemSubscription;

public record CreateSystemSubscriptionUseCase(SystemSubscriptionForCreationDto SubscriptionForCreationDto):IRequest<string>;

public class CreateSystemSubscriptionUseCaseHandler : IRequestHandler<CreateSystemSubscriptionUseCase, string>
{
    private readonly IPrimaryServiceManager _primaryServiceManager;

    public CreateSystemSubscriptionUseCaseHandler(IPrimaryServiceManager primaryServiceManager)
    {
        _primaryServiceManager = primaryServiceManager;
    }
    public async Task<string> Handle(CreateSystemSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        return await _primaryServiceManager.SystemSubscriptionService.CreateSystemSubscription(request.SubscriptionForCreationDto);
    }
}