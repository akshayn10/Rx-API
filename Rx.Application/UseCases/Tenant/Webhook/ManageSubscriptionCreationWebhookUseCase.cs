using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Webhook;

public record ManageSubscriptionCreationWebhookUseCase(SubscriptionWebhookForCreationDto SubscriptionWebhookForCreationDto):IRequest<string>;

public class CreateSubscriptionFromWebhookUseCaseHandler : IRequestHandler<ManageSubscriptionCreationWebhookUseCase, string>
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
    public async Task<string> Handle(ManageSubscriptionCreationWebhookUseCase request, CancellationToken cancellationToken)
    {
        //Store Webhook in Database
        var subscriptionWebhookDto = new SubscriptionWebhookDto(
            SenderWebhookId:request.SubscriptionWebhookForCreationDto.SenderWebhookId,
            CustomerEmail:request.SubscriptionWebhookForCreationDto.customerEmail,
            CustomerName:request.SubscriptionWebhookForCreationDto.customerName,
            ProductPlanId:request.SubscriptionWebhookForCreationDto.productPlanId,
            RetrievedDate:DateTime.Now,
            SubscriptionType:request.SubscriptionWebhookForCreationDto.subscriptionType
        );
        var subscriptionWebhook = _mapper.Map<SubscriptionWebhook>(subscriptionWebhookDto);
        await _tenantDbContext.SubscriptionWebhooks.AddAsync(subscriptionWebhook, cancellationToken);
        await _tenantDbContext.SaveChangesAsync();
        
        //Check if the customer is new or existing
        var customer =await _tenantDbContext.OrganizationCustomers!.FirstOrDefaultAsync(c=>c.Email==request.SubscriptionWebhookForCreationDto.customerEmail, cancellationToken: cancellationToken);
        if (customer != null)
        {
            // Create new function for existing customer
            return await _tenantServiceManager.SubscriptionService.CreateSubscriptionFromWebhook(customer.CustomerId);
        }
        return await _tenantServiceManager.OrganizationCustomerService.CreateCustomerFromWebhook(request.SubscriptionWebhookForCreationDto);
    }
}