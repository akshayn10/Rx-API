using System.Net.Http.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Rx.Domain.DTOs.Payment;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Payment;
using Rx.Domain.Interfaces.Tenant;
using Rx.Domain.Interfaces.WebhookSendClient;
using Stripe;

namespace Rx.Domain.Services.Tenant;

public class AddOnUsageService: IAddOnUsageService
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly ILogger<ITenantServiceManager> _logger;
    private readonly IMapper _mapper;
    private readonly IPaymentService _paymentService;
    private readonly ISendWebhookService _sendWebhookService;

    public AddOnUsageService(ITenantDbContext tenantDbContext,
        ILogger<ITenantServiceManager> logger,
        IMapper mapper,
        IPaymentService paymentService,
        ISendWebhookService sendWebhookService) 
    {
        _tenantDbContext = tenantDbContext;
        _logger = logger;
        _mapper = mapper;
        _paymentService = paymentService;
        _sendWebhookService = sendWebhookService;
    }
    public async Task<AddOnUsageDto> CreateAddOnUsage(Guid subscriptionId, Guid addOnId, AddOnUsageForCreationDto addOnUsageForCreationDto)
    {
        var addOnUsage = _mapper.Map<AddOnUsage>(addOnUsageForCreationDto);
        addOnUsage.Date=DateTime.Now;
        var subscription = await _tenantDbContext.Subscriptions!.FindAsync(subscriptionId);
        var addOn = await _tenantDbContext.AddOns!.FindAsync(addOnId);
        if(subscription==null || addOn==null)
        {
            throw new Exception("Subscription or AddOn not found");
        }
        await _tenantDbContext.AddOnUsages!.AddAsync(addOnUsage);
        await _tenantDbContext.SaveChangesAsync();
        return _mapper.Map<AddOnUsageDto>(addOnUsage);
        
    }

    public async Task<string> CreateAddOnUsageFromWebhook(AddOnWebhook addOnWebhook)
    {
        var subscription =await _tenantDbContext.Subscriptions!.FindAsync(addOnWebhook.SubscriptionId);
        if(subscription==null)
        {
            throw new NullReferenceException("Subscription not found");
        }

        var customer =await _tenantDbContext.OrganizationCustomers!.FindAsync(addOnWebhook.OrganizationCustomerId);
        var addOnPriceForPlan =await _tenantDbContext.AddOnPricePerPlans!.Where(
                ap=>ap.AddOnId==addOnWebhook.AddOnId 
                    && ap.ProductPlanId==subscription.ProductPlanId)
                .FirstOrDefaultAsync();
        if (addOnPriceForPlan == null)
        {
            throw new NullReferenceException("AddOnPricePerPlan not found");
        }
        var unitInDecimal = Convert.ToDecimal(addOnWebhook.Unit);
        var amountToBeCharged = addOnPriceForPlan.Price*unitInDecimal;
        var stripeDescription = new StripeDescription("addOn",addOnWebhook.AddOnWebhookId.ToString());
        var stripeDescriptionJson = JsonConvert.SerializeObject(stripeDescription,Formatting.Indented);
       
        await _paymentService.Charge(
            customer!.PaymentGatewayId!,
            customer.PaymentMethodId!,
            PaymentModel.Currency.USD,
            Convert.ToInt64(amountToBeCharged),
            customer.Email!,
            false,
            stripeDescriptionJson
        );
        
        return "Payment Processing";
    }

    public async Task<string> ActivateAddOnUsageAfterPayment(string webhookId,long amount)
    {
        var webhook = await _tenantDbContext.AddOnWebhooks.FindAsync(Guid.Parse(webhookId));
        var addOnUsageDto = new AddOnUsageForCreationDto(
            Unit:webhook!.Unit,
            AddOnId:webhook.AddOnId,
            SubscriptionId:webhook.SubscriptionId,
            TotalAmount:Convert.ToDecimal(amount)
        );
        var addOnUsage = _mapper.Map<AddOnUsage>(addOnUsageDto);
        addOnUsage.Date=DateTime.Now;
        await _tenantDbContext.AddOnUsages!.AddAsync(addOnUsage);
        await _tenantDbContext.SaveChangesAsync();
        
        //Response to Backends
        var backendAddOnResponse = new BackendAddOnResponse(
            "AddOnActivated",webhook.OrganizationCustomerId.ToString(), webhook.SubscriptionId.ToString(),webhook.AddOnId.ToString());
        await _sendWebhookService.SendAddOnWebhookToProductBackend(backendAddOnResponse);
        return addOnUsage.AddOnUsageId.ToString();
    }
}