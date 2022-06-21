using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rx.Domain.DTOs.Email;
using Rx.Domain.DTOs.Payment;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.DTOs.Primary.SystemSubscription;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Email;
using Rx.Domain.Interfaces.Payment;
using Rx.Domain.Interfaces.Primary;

namespace Rx.Domain.Services.Primary;

public class SystemSubscriptionService:ISystemSubscriptionService
{
    private readonly IPrimaryDbContext _primaryDbContext;
    private readonly ILogger<PrimaryServiceManager> _logger;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IPaymentService _paymentService;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public SystemSubscriptionService(IPrimaryDbContext primaryDbContext, ILogger<PrimaryServiceManager> logger,
        IMapper mapper,IEmailService emailService,IPaymentService paymentService,IBackgroundJobClient backgroundJobClient)
    {
        _primaryDbContext = primaryDbContext;
        _logger = logger;
        _mapper = mapper;
        _emailService = emailService;
        _paymentService = paymentService;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task<string> CreateSystemSubscription(SystemSubscriptionForCreationDto subscriptionForCreationDto)
    {
        var subscription = new SystemSubscription
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(1),
            CreatedDate = DateTime.Now,
            SubscriptionType = subscriptionForCreationDto.SubscriptionType,
            IsActive = false,
            IsCancelled = false,
            OrganizationId = subscriptionForCreationDto.OrganizationId,
            SystemSubscriptionPlanId = subscriptionForCreationDto.SystemSubscriptionPlanId
        };
        await _primaryDbContext.SystemSubscriptions!.AddAsync(subscription);
        await _primaryDbContext.SaveChangesAsync();
        
        
        var organization = await _primaryDbContext.Organizations!.FindAsync(subscriptionForCreationDto.OrganizationId);
        if(organization == null)
            throw new Exception("Organization not found");
        var plan = await _primaryDbContext.SystemSubscriptionPlans.FindAsync(subscriptionForCreationDto
            .SystemSubscriptionPlanId);
        if(plan == null)
            throw new Exception("Plan not found");
        
        if (subscription.SubscriptionType == false)
        {

            var stripeDescription =
                new StripeDescription("organization-onetime", subscription.SubscriptionId.ToString());
            var stripeDescriptionJson = JsonConvert.SerializeObject(stripeDescription);

            await _paymentService.Charge(
                organization!.PaymentGatewayId!,
                organization.PaymentMethodId!,
                PaymentModel.Currency.USD,
                Convert.ToInt64(plan!.Price),
                organization.Email!,
                false,
                stripeDescriptionJson
            );
            return "Payment Processing for Organization Onetime";
        }

        if (subscription.SubscriptionType)
        {
            var stripeDescription =
                new StripeDescription("organization-recurring", subscription.SubscriptionId.ToString());
            var stripeDescriptionJson = JsonConvert.SerializeObject(stripeDescription);

            await _paymentService.Charge(
                organization!.PaymentGatewayId!,
                organization.PaymentMethodId!,
                PaymentModel.Currency.USD,
                Convert.ToInt64(plan!.Price),
                organization.Email!,
                false,
                stripeDescriptionJson
            );
            return "Payment Processing for Organization Recurring";
        }
        return "Failed processing payment";
    }


    public async Task<string> DeactivateSystemSubscription(Guid subscriptionId)
    {
        var subscription = await _primaryDbContext.SystemSubscriptions!.FindAsync(subscriptionId);
        subscription!.IsActive = false;
        await _primaryDbContext.SaveChangesAsync();
        return "Subscription deactivated for organization " + subscription.OrganizationId;
    }
    public async Task<string> CancelSubscription(Guid subscriptionId)
    {
        var subscription = await _primaryDbContext.SystemSubscriptions!.FindAsync(subscriptionId);
        if (subscription == null)
        {
            throw new Exception("Subscription not found");
        }
        subscription!.IsActive = false;
        subscription!.IsCancelled = true;
        await _primaryDbContext.SaveChangesAsync();
        var organization =await _primaryDbContext.Organizations!.FindAsync(subscription.OrganizationId);
        _backgroundJobClient.Delete(subscription.JobId);
        
        var emailRequest = new EmailRequest()
        {
            To = organization!.Email,
            Subject = "Subscription Cancelled",
            Body = $"System Onetime Subscription Activated for {organization.Name} at {DateTime.Now}"
        };
        _backgroundJobClient.Enqueue(()=>_emailService.SendAsync(emailRequest));

        return "Subscription Cancelled for organization " + subscription.OrganizationId;
    }

    public async Task<string> ActivateOneTimeSubscription(Guid subscriptionId)
    {
        var subscription = await _primaryDbContext.SystemSubscriptions!.FindAsync(subscriptionId);
        subscription!.IsActive = true;
        var jobId = _backgroundJobClient.Schedule(() => DeactivateSystemSubscription(subscriptionId),DateTime.Now.AddMonths(1));
        subscription.JobId = jobId;
        await _primaryDbContext.SaveChangesAsync();
        var organization = await _primaryDbContext.Organizations!.FindAsync(subscription.OrganizationId);
        
        var emailRequest = new EmailRequest()
        {
            To = organization!.Email,
            Subject = "Subscription Activated",
            Body = $"System Onetime Subscription Activated for {organization.Name} at {DateTime.Now}"
        };
        _backgroundJobClient.Enqueue(()=>_emailService.SendAsync(emailRequest));
        
        return "One time subscription activated for organization " + subscription.OrganizationId;
    }

    public async Task<string> ActivateRecurringSubscription(Guid subscriptionId)
    {
        var subscription = await _primaryDbContext.SystemSubscriptions!.FindAsync(subscriptionId);
        subscription!.IsActive = true;
        var jobId = _backgroundJobClient.Schedule(() => RecurringSubscription(subscriptionId),DateTime.Now.AddMonths(1));
        subscription.JobId = jobId;
        await _primaryDbContext.SaveChangesAsync();
        var organization = await _primaryDbContext.Organizations!.FindAsync(subscription.OrganizationId);
        
        var emailRequest = new EmailRequest()
        {
            To = organization!.Email,
            Subject = "Subscription Activated",
            Body = $"System Recurring Subscription Activated for {organization.Name} at {DateTime.Now}"
        };
        _backgroundJobClient.Enqueue(()=>_emailService.SendAsync(emailRequest));
        
        return "One time subscription activated for organization " + subscription.OrganizationId;
    }

    public async Task<string> RecurringSubscription(Guid subscriptionId)
    {
        var subscription = await _primaryDbContext.SystemSubscriptions!.FindAsync(subscriptionId);
        subscription!.IsActive = false;
        await _primaryDbContext.SaveChangesAsync();
        //Deserialize Description
        var stripeDescription = new StripeDescription("organization-recurring-period", subscription.SubscriptionId.ToString());
        var stripeDescriptionJson = JsonConvert.SerializeObject(stripeDescription);
            
        var organization =await _primaryDbContext.Organizations!.FindAsync(subscription.OrganizationId);
        if(organization == null)
            throw new Exception("Organization not found");
        var plan = await _primaryDbContext.SystemSubscriptionPlans!.FindAsync(subscription.SystemSubscriptionPlanId);
        if(plan == null)
            throw new Exception("Plan not found");
            
        await _paymentService.Charge(
            organization!.PaymentGatewayId!,
            organization.PaymentMethodId!,
            PaymentModel.Currency.USD,
            Convert.ToInt64(plan!.Price),
            organization.Email!,
            false,
            stripeDescriptionJson
        );
        _logger.LogInformation("Processing Payment For Recurring");
        return subscription.SubscriptionId.ToString();
    }

    public async Task<string> ActivatePeriodRecurringSubscription(Guid subscriptionId)
    {
        
        var subscription =await _primaryDbContext.SystemSubscriptions!.FindAsync(subscriptionId);
        if(subscription is null)
        {
            throw new NullReferenceException("Subscription not found");
        }
        var plan = await _primaryDbContext.SystemSubscriptionPlans!.FindAsync(subscription.SystemSubscriptionPlanId);
        if(plan is null)
        {
            throw new NullReferenceException("Plan not found");
        }
        var organization=  await _primaryDbContext.Organizations!.FindAsync(subscription.OrganizationId);
            
        subscription.StartDate = DateTime.Now;
        var jobId = _backgroundJobClient.Schedule(() => RecurringSubscription(subscriptionId),
            subscription.StartDate.AddMonths(1));
        subscription.JobId = jobId;
        subscription.IsActive = true;
        await _primaryDbContext.SaveChangesAsync();
        //Store Transaction
        var emailRequest = new EmailRequest()
        {
            To = organization!.Email,
            Subject = "Subscription ReActivated",
            Body = $"System Recurring Subscription Activated for {organization.Name} at {DateTime.Now}"
        };
        _backgroundJobClient.Enqueue(()=>_emailService.SendAsync(emailRequest));
        
        return "One time subscription activated for organization " + subscription.OrganizationId;
    }
}