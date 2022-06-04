using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Webhook;

public record ManageWebhookUseCase(SubscriptionWebhookForCreationDto SubscriptionWebhookForCreationDto):IRequest<string>;

public class CreateSubscriptionFromWebhookUseCaseHandler : IRequestHandler<ManageWebhookUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;
    private readonly ITenantDbContext _tenantDbContext;
    private readonly IMapper _mapper;

    public CreateSubscriptionFromWebhookUseCaseHandler(ITenantServiceManager tenantServiceManager,ITenantDbContext tenantDbContext,IMapper mapper)
    {
        _tenantServiceManager = tenantServiceManager;
        _tenantDbContext = tenantDbContext;
        _mapper = mapper;
    }
    public async Task<string> Handle(ManageWebhookUseCase request, CancellationToken cancellationToken)
    {
        var subscriptionWebhookDto = new SubscriptionWebhookDto(
            request.SubscriptionWebhookForCreationDto.SenderWebhookId,
            request.SubscriptionWebhookForCreationDto.customerEmail,
            request.SubscriptionWebhookForCreationDto.customerName,
            request.SubscriptionWebhookForCreationDto.productPlanId);
        var subscriptionWebhook = _mapper.Map<SubscriptionWebhook>(subscriptionWebhookDto);

        var customer =await _tenantDbContext.OrganizationCustomers!.FirstOrDefaultAsync(c=>c.Email==request.SubscriptionWebhookForCreationDto.customerEmail, cancellationToken: cancellationToken);
        if (customer != null)
        {
           await _tenantServiceManager.OrganizationCustomerService.CreateCustomerFromWebhook(request.SubscriptionWebhookForCreationDto);
        }
        return await _tenantServiceManager.SubscriptionService.CreateSubscriptionFromWebhook(customer.CustomerId);
    }
}