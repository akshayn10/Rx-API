using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Tenant.AddOn;
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

        var customer =await _tenantDbContext.OrganizationCustomers!.FindAsync(subscription.OrganizationCustomerId);
        var addOnPriceForPlan =await 
            _tenantDbContext.AddOnPricePerPlans!.Where(
                ap=>ap.AddOnId==addOnUsageFromWebhookForCreationDto.AddOnId 
                    && ap.ProductPlanId==subscription.ProductPlanId)
                .FirstOrDefaultAsync();
        var amountToBeCharged= addOnPriceForPlan!.Price*addOnUsageFromWebhookForCreationDto.Unit;
       
        var paymentResponse = await _paymentService.Charge(
            customer!.PaymentGatewayId!,
            customer.PaymentMethodId!,
            DTOs.Payment.PaymentModel.Currency.USD,
            Convert.ToInt64(amountToBeCharged),
            customer.Email!,
            false,
            ""
            
            );
        if (paymentResponse == "succeeded")
        {
            var addOnUsageDto = new AddOnUsageForCreationDto(
                Unit:addOnUsageFromWebhookForCreationDto.Unit,
                AddOnId:addOnUsageFromWebhookForCreationDto.AddOnId,
                SubscriptionId:addOnUsageFromWebhookForCreationDto.SubscriptionId
            );
            var addOnUsage = _mapper.Map<AddOnUsage>(addOnUsageDto);
            addOnUsage.Date=DateTime.Now;
            await _tenantDbContext.AddOnUsages!.AddAsync(addOnUsage);
            await _tenantDbContext.SaveChangesAsync();
            return addOnUsage.AddOnUsageId.ToString();
        }
        return "AddOn Subscription Failed";
    }
}