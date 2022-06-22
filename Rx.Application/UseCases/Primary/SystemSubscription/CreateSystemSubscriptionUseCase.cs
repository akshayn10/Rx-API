using MediatR;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.DTOs.Primary.SystemSubscription;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Primary.SystemSubscription;

public record CreateSystemSubscriptionUseCase(SystemSubscriptionForCreationDto SubscriptionForCreationDto):IRequest<string>;

public class CreateSystemSubscriptionUseCaseHandler : IRequestHandler<CreateSystemSubscriptionUseCase, string>
{
    private readonly IPrimaryServiceManager _primaryServiceManager;
    private readonly IPrimaryDbContext _primaryDbContext;

    public CreateSystemSubscriptionUseCaseHandler(IPrimaryServiceManager primaryServiceManager,IPrimaryDbContext primaryDbContext)
    {
        _primaryServiceManager = primaryServiceManager;
        _primaryDbContext = primaryDbContext;
    }
    public async Task<string> Handle(CreateSystemSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        var subscriptionReq = new SubscriptionRequest
        {
            RetrievedDateTime = DateTime.Now,
            OrganizationId = request.SubscriptionForCreationDto.OrganizationId,
            SystemPlanId = request.SubscriptionForCreationDto.SystemSubscriptionPlanId,
            SubscriptionType = request.SubscriptionForCreationDto.SubscriptionType
        };
        await _primaryDbContext.SubscriptionRequests.AddAsync(subscriptionReq);
        await _primaryDbContext.SaveChangesAsync();
        var organization =await _primaryDbContext.Organizations!.FindAsync(request.SubscriptionForCreationDto.OrganizationId);
        if (organization == null)
        {
            throw new System.Exception("Organization not found");
        }
        if(organization.PaymentGatewayId == null)
        {
            return await _primaryServiceManager.OrganizationService.CreateOrganizationInStripeUseCase(subscriptionReq);
        }
        
        return await _primaryServiceManager.SystemSubscriptionService.CreateSystemSubscription(request.SubscriptionForCreationDto);
    }
}