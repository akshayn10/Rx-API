using MediatR;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Subscription;

public record DowngradeSubscriptionUseCase(ChangeSubscriptionWebhookDto ChangeSubscriptionWebhookDto):IRequest<string>;

public class DowngradeSubscriptionUseCaseHandler: IRequestHandler<DowngradeSubscriptionUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;
    private readonly ITenantDbContext _tenantDbContext;

    public DowngradeSubscriptionUseCaseHandler(ITenantServiceManager tenantServiceManager,ITenantDbContext tenantDbContext)
    {
        _tenantServiceManager = tenantServiceManager;
        _tenantDbContext = tenantDbContext;
    }
    public async Task<string> Handle(DowngradeSubscriptionUseCase request, CancellationToken cancellationToken)
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

        return await _tenantServiceManager.SubscriptionService.DowngradeSubscription(changeSubscriptionWebhook);

    }
}