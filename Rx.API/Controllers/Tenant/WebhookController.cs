using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.API.Controllers.Tenant.Authorization;
using Rx.Domain.Entities.Tenant;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Create Subscription webhooks")]
        public async Task<IActionResult> SubscribeWebhook([FromBody] SubscriptionWebhook subscriptionWebhook)
        {
            var data = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret =Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + data);
            _recurringJobManager.AddOrUpdate("jobId",()=>Console.WriteLine("ejgenroi"),Cron.Minutely); ;
            return Ok();
        }
        [HttpPost("unsubscribe",Name = "UnsubscribeWebhook")]
        [SwaggerOperation(Summary = "Unsubscribe webhook")]
        public async Task<IActionResult> UnsubscribeWebhook([FromBody] SubscriptionWebhook subscriptionWebhook)
        {
            var data = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + data);
            return Ok();
        }
        [HttpPost("upgradeSubscription",Name = "UpgradeSubscriptionWebhook")]
        [SwaggerOperation(Summary = "Upgrade Subscription webhook")]
        public async Task<IActionResult> UpgradeSubscriptionWebhook([FromBody] SubscriptionWebhook subscriptionWebhook)
        {
            var data = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + data);
            return Ok();
        }
        [HttpPost("downgradeSubscription",Name = "DowngradeSubscriptionWebhook")]
        [SwaggerOperation(Summary = "Downgrade Subscription webhook")]
        public async Task<IActionResult> DowngradeSubscriptionWebhook([FromBody] SubscriptionWebhook subscriptionWebhook)
        {
            var data = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + data);
            return Ok();
        }
        [HttpPost("addOn")]
        [SwaggerOperation(Summary = "Add On webhook")]
        public async Task<IActionResult> AddOnWebhook([FromBody] AddOnWebhook addOnWebhook)
        {
            var data = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + data);
            return Ok();
        }
    }
}
