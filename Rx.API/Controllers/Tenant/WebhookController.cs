using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.API.Controllers.Tenant.Authorization;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/webhook")]
    [ApiController]
    [WebhookVerification]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly IMediator _mediator;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        public WebhookController(ILogger<WebhookController> logger, IMediator mediator, IBackgroundJobClient backgroundJobClient,IRecurringJobManager recurringJobManager)
        {
            _logger = logger;
            _mediator = mediator;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }
        
        [HttpPost("subscribe",Name = "SubscriptionWebhook")]
        public async Task<IActionResult> SubscribeWebhook()
        {
            var data = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Guid.Parse(Request.Headers["Secret"]);
            _logger.LogInformation(secret + " " + tenantId + " " + data);
            _recurringJobManager.AddOrUpdate("jobId",()=>Console.WriteLine("ejgenroi"),Cron.Minutely); ;
            return Ok();
        }
        [HttpPost("unsubscribe",Name = "UnsubscribeWebhook")]
        public async Task<IActionResult> UnsubscribeWebhook()
        {
            var data = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Guid.Parse(Request.Headers["Secret"]);
            _logger.LogInformation(secret + " " + tenantId + " " + data);
            return Ok();
        }
        [HttpPost("upgradeSubscription",Name = "UpgradeSubscriptionWebhook")]
        public async Task<IActionResult> UpgradeSubscriptionWebhook()
        {
            var data = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Guid.Parse(Request.Headers["Secret"]);
            _logger.LogInformation(secret + " " + tenantId + " " + data);
            return Ok();
        }
        [HttpPost("downgradeSubscription",Name = "DowngradeSubscriptionWebhook")]
        public async Task<IActionResult> DowngradeSubscriptionWebhook()
        {
            var data = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Guid.Parse(Request.Headers["Secret"]);
            _logger.LogInformation(secret + " " + tenantId + " " + data);
            return Ok();
        }
    }
}
