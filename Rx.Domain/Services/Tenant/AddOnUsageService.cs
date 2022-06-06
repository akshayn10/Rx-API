using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Payment;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Payment;
using Rx.Domain.Interfaces.Tenant;
using Stripe;

namespace Rx.Domain.Services.Tenant;

public class AddOnUsageService: IAddOnUsageService
{
    private readonly ITenantDbContext _tenantDbContext;
    private readonly ILogger<ITenantServiceManager> _logger;
    private readonly IMapper _mapper;
    private readonly IPaymentService _paymentService;

    public AddOnUsageService(ITenantDbContext tenantDbContext,ILogger<ITenantServiceManager> logger,IMapper mapper,IPaymentService paymentService)
    {
        _tenantDbContext = tenantDbContext;
        _logger = logger;
        _mapper = mapper;
        _paymentService = paymentService;
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

    public async Task<string> CreateAddOnUsageFromWebhook(AddOnUsageFromWebhookForCreationDto addOnUsageFromWebhookForCreationDto)
    {
        var subscription =await _tenantDbContext.Subscriptions!.FindAsync(addOnUsageFromWebhookForCreationDto.SubscriptionId);
        if(subscription==null)
        {
            throw new NullReferenceException("Subscription not found");
        }

        var customer =await _tenantDbContext.OrganizationCustomers!.FindAsync(addOnUsageFromWebhookForCreationDto.CustomerId);
        var addOnPriceForPlan =await _tenantDbContext.AddOnPricePerPlans!.Where(
                ap=>ap.AddOnId==addOnUsageFromWebhookForCreationDto.AddOnId 
                    && ap.ProductPlanId==subscription.ProductPlanId)
                .FirstOrDefaultAsync();
        if (addOnPriceForPlan == null)
        {
            throw new NullReferenceException("AddOnPricePerPlan not found");
        }
        var unitInDecimal = Convert.ToDecimal(addOnUsageFromWebhookForCreationDto.Unit);
        var amountToBeCharged = addOnPriceForPlan.Price*unitInDecimal;
       
        await _paymentService.Charge(
            customer!.PaymentGatewayId!,
            customer.PaymentMethodId!,
            PaymentModel.Currency.USD,
            Convert.ToInt64(amountToBeCharged),
            customer.Email!,
            false,
            "addOn"
        );

        
        return "Payment Processing";
    }

    public async Task<string> ActivateAddOnUsageAfterPayment(string customerId)
    {
        var customer =await _tenantDbContext.OrganizationCustomers!.FirstOrDefaultAsync(c=>c.PaymentGatewayId==customerId);
        var lastAddOnWebhook= await _tenantDbContext.AddOnWebhooks.Where(aw=>aw.OrganizationCustomerId==customer!.CustomerId).OrderByDescending(aw=>aw.RetrievedDateTime).FirstOrDefaultAsync();
        var addOnUsageDto = new AddOnUsageForCreationDto(
            Unit:lastAddOnWebhook!.Unit,
            AddOnId:lastAddOnWebhook.AddOnId,
            SubscriptionId:lastAddOnWebhook.SubscriptionId
        );
        var addOnUsage = _mapper.Map<AddOnUsage>(addOnUsageDto);
        addOnUsage.Date=DateTime.Now;
        await _tenantDbContext.AddOnUsages!.AddAsync(addOnUsage);
        await _tenantDbContext.SaveChangesAsync();
        return addOnUsage.AddOnUsageId.ToString();
    }
}