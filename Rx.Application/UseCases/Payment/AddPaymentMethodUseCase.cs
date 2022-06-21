using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Application.UseCases.Tenant.Subscription;
using Rx.Domain.DTOs.Primary.SystemSubscription;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Payment;

public record AddPaymentMethodUseCase(string CustomerId,string Last4,string PaymentMethodId):IRequest;

public class AddPaymentMethodUseCaseHandler : IRequestHandler<AddPaymentMethodUseCase>
{
    private readonly ITenantServiceManager _tenantServiceManager;
    private readonly IPrimaryDbContext _primaryDbContext;
    private readonly IPrimaryServiceManager _primaryServiceManager;
    private readonly IMediator _mediator;

    public AddPaymentMethodUseCaseHandler(ITenantServiceManager tenantServiceManager,IPrimaryDbContext primaryDbContext,IPrimaryServiceManager primaryServiceManager,IMediator mediator)
    {
        _tenantServiceManager = tenantServiceManager;
        _primaryDbContext = primaryDbContext;
        _primaryServiceManager = primaryServiceManager;
        _mediator = mediator;
    }
    public async Task<Unit> Handle(AddPaymentMethodUseCase request, CancellationToken cancellationToken)
    {
        var organization = await _primaryDbContext.Organizations!.FirstOrDefaultAsync(o=>o.PaymentGatewayId==request.CustomerId);
        if (organization!=null)
        {
            var storedSubscriptionId = await _primaryServiceManager.OrganizationService.AddPaymentMethodIdForOrganization(organization.Id,request.PaymentMethodId);
            var storedSubscription = await _primaryDbContext.SubscriptionRequests!.FindAsync(storedSubscriptionId);
            var subscriptionCreationDto = new SystemSubscriptionForCreationDto(storedSubscription!.SubscriptionType,
                storedSubscription.OrganizationId, storedSubscription.SystemPlanId);
            await _primaryServiceManager.SystemSubscriptionService.CreateSystemSubscription(subscriptionCreationDto);
            return Unit.Value;
            
        }
        var webhookId =await _tenantServiceManager.OrganizationCustomerService.AddPaymentMethod(request.CustomerId, request.Last4,request.PaymentMethodId);
        await _mediator.Send(new CreateSubscriptionFromWebhookUseCase(webhookId));
        return Unit.Value;
    }
}