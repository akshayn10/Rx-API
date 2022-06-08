using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.DTOs.Tenant.Subscription;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.WebhookSendClient;
namespace Rx.Domain.Services.WebhookSendClient;

public class SendWebhookService:ISendWebhookService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITenantDbContext _tenantDbContext;
    private readonly ILogger<SendWebhookService> _logger;
    private HttpClient? _httpClient;
    private static RetryPolicy? _retryPolicy;

    public SendWebhookService(IHttpClientFactory httpClientFactory,ITenantDbContext tenantDbContext,ILogger<SendWebhookService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _tenantDbContext = tenantDbContext;
        _logger = logger;
        _retryPolicy = Policy
            .Handle<Exception>()
            .Retry(2);
    }
    public async Task SendSubscriptionWebhookToProductBackend(BackendSubscriptionResponse backendSubscriptionResponse)
    {
        var plan =await _tenantDbContext.ProductPlans!.Include(pp=>pp.Product).FirstOrDefaultAsync(pp =>
            pp.PlanId == Guid.Parse(backendSubscriptionResponse.PlanId));
        if (plan == null)
        {
            throw new NullReferenceException("Plan not found");
            
        }
        var webHookSecret = plan!.Product!.WebhookSecret??"DummySecret";
        // var webhookUrlToSend = plan.Product.WebhookURL??"https://baeb0b32f6296cd6566129eed5eb1a12.m.pipedream.net";
        var webhookUrlToSend = "https://baeb0b32f6296cd6566129eed5eb1a12.m.pipedream.net";
        _logger.LogInformation(webHookSecret+" "+webhookUrlToSend);
        
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("ApiKey", webHookSecret);
        
        var response = await _retryPolicy!.Execute(()=> _httpClient.PostAsJsonAsync(webhookUrlToSend,backendSubscriptionResponse ));
        _logger.LogInformation(response.StatusCode.ToString());
    }
    public async Task SendAddOnWebhookToProductBackend(BackendAddOnResponse backendAddOnResponse)
    {
        var subscription =await _tenantDbContext.Subscriptions!.FindAsync(Guid.Parse(backendAddOnResponse.SubscriptionId));
        var plan =await _tenantDbContext.ProductPlans!.Include(pp=>pp.Product).FirstOrDefaultAsync(pp =>
            pp.PlanId == subscription!.ProductPlanId);
        if (plan == null)
        {
            throw new NullReferenceException("Plan not found");
            
        }
        var webHookSecret = plan!.Product!.WebhookSecret??"DummySecret";
        // var webhookUrlToSend = plan.Product.WebhookURL??"https://baeb0b32f6296cd6566129eed5eb1a12.m.pipedream.net";
        var webhookUrlToSend = "https://baeb0b32f6296cd6566129eed5eb1a12.m.pipedream.net";
        _logger.LogInformation(webHookSecret+" "+webhookUrlToSend);
        
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("ApiKey", webHookSecret);
        var response = await _retryPolicy!.Execute(()=> _httpClient.PostAsJsonAsync(webhookUrlToSend,backendAddOnResponse ));
        _logger.LogInformation(response.StatusCode.ToString());
    }
}