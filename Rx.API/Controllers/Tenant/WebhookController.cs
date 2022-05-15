using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.API.Controllers.Tenant.Authorization;
using Rx.Application.UseCases.Tenant.Subscription;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.Subscription;
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
        public WebhookController(ILogger<WebhookController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        // Retrieve Subscription Webhooks
        [HttpPost("subscribe",Name = "SubscriptionWebhook")]
        [SwaggerOperation(Summary = "Create Subscription webhooks")]
        public async Task<IActionResult> SubscribeWebhook([FromBody] SubscriptionWebhookForCreationDto subscriptionWebhookForCreationDto)
        {
            var tenantId = Request.Headers["TenantId"];
            var secret =Request.Headers["Secret"];
            var subscription =await _mediator.Send(new CreateSubscriptionFromWebhookUseCase(subscriptionWebhookForCreationDto));
            return Ok(subscription);
        }
        [HttpPost("unsubscribe",Name = "UnsubscribeWebhook")]
        [SwaggerOperation(Summary = "Unsubscribe webhook")]
        public async Task<IActionResult> UnsubscribeWebhook([FromBody] SubscriptionWebhookForCreationDto subscriptionWebhookForCreationDto)
        {
            var tenantId = Request.Headers["TenantId"];
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + subscriptionWebhookForCreationDto);
            return Ok();
        }
        [HttpPost("upgradeSubscription",Name = "UpgradeSubscriptionWebhook")]
        [SwaggerOperation(Summary = "Upgrade Subscription webhook")]
        public async Task<IActionResult> UpgradeSubscriptionWebhook([FromBody] SubscriptionWebhookForCreationDto subscriptionWebhookForCreationDto)
        {
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + subscriptionWebhookForCreationDto);
            return Ok();
        }
        [HttpPost("downgradeSubscription",Name = "DowngradeSubscriptionWebhook")]
        [SwaggerOperation(Summary = "Downgrade Subscription webhook")]
        public async Task<IActionResult> DowngradeSubscriptionWebhook([FromBody] SubscriptionWebhookForCreationDto subscriptionWebhookForCreationDto)
        {
            var tenantId = Request.Headers["TenantId"];
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + subscriptionWebhookForCreationDto);
            return Ok();
        }
        
        //Retrieve AddOn Webhooks
        [HttpPost("addOn")]
        [SwaggerOperation(Summary = "Add On webhook")]
        public async Task<IActionResult> AddOnWebhook([FromBody] AddOnWebhookForCreationDto addOnWebhookForCreationDto)
        {
            var tenantId = Request.Headers["TenantId"];
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + addOnWebhookForCreationDto);
            return Ok();
        }
    }
}
