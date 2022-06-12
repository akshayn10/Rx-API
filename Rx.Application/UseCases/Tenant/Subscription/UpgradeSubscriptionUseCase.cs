using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record UpgradeSubscriptionUseCase(ChangeSubscriptionWebhookDto ChangeSubscriptionWebhookDto):IRequest<string>;

public class UpgradeSubscriptionUseCaseHandler : IRequestHandler<UpgradeSubscriptionUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;
    private readonly ITenantDbContext _tenantDbContext;

    public UpgradeSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager,ITenantDbContext tenantDbContext)
    {
        _tenantServiceManager = tenantServiceManager;
        _tenantDbContext = tenantDbContext;
    }
    public async Task<string> Handle(UpgradeSubscriptionUseCase request, CancellationToken cancellationToken)
    {
        var changeSubscriptionWebhook = new ChangeSubscriptionWebhook()
        {
            SenderWebhookId = request.ChangeSubscriptionWebhookDto.SenderWebhookId,
            OldSubscriptionId = request.ChangeSubscriptionWebhookDto.SubscriptionId,
            NewPlanId = request.ChangeSubscriptionWebhookDto.PlanId,
            NewSubscriptionType = request.ChangeSubscriptionWebhookDto.NewSubscriptionType,
            CustomerId = request.ChangeSubscriptionWebhookDto.CustomerId
        };
        await _tenantDbContext.ChangeSubscriptionWebhooks.AddAsync(changeSubscriptionWebhook, cancellationToken);
        await _tenantDbContext.SaveChangesAsync();
        
        return await _tenantServiceManager.SubscriptionService.UpgradeSubscription(changeSubscriptionWebhook);
    }
}

