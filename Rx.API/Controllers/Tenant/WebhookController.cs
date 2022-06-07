using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.API.Controllers.Tenant.Authorization;
using Rx.Application.UseCases.Tenant.Subscription;
using Rx.Application.UseCases.Tenant.Webhook;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.DTOs.Tenant.Subscription;
using Stripe;
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
        [HttpPost("subscribe")]
        [SwaggerOperation(Summary = "Create Subscription webhooks")]
        public async Task<IActionResult> SubscribeWebhook([FromBody] SubscriptionWebhookForCreationDto subscriptionWebhookForCreationDto)
        {
            var tenantId = Request.Headers["TenantId"];
            var secret =Request.Headers["Secret"];
            var paymentRedirectUrl =await _mediator.Send(new ManageSubscriptionCreationWebhookUseCase(subscriptionWebhookForCreationDto));
            return Ok(paymentRedirectUrl);
        }
        [HttpPost("unsubscribe")]
        [SwaggerOperation(Summary = "Unsubscribe webhook")]
        public async Task<IActionResult> UnsubscribeWebhook([FromBody] UnsubscriptionWebhookDto unsubscriptionWebhookDto)
        {
            var tenantId = Request.Headers["TenantId"];
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + unsubscriptionWebhookDto);
            await _mediator.Send(new UnsubscribeUseCase(unsubscriptionWebhookDto));
            return Ok();
        }
        [HttpPost("upgradeSubscription")]
        [SwaggerOperation(Summary = "Upgrade Subscription webhook")]
        public async Task<IActionResult> UpgradeSubscriptionWebhook([FromBody] ChangeSubscriptionWebhookDto changeSubscriptionWebhookDto)
        {
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + changeSubscriptionWebhookDto);
            await _mediator.Send(new UpgradeSubscriptionUseCase(changeSubscriptionWebhookDto));
            return Ok();
        }
        [HttpPost("downgradeSubscription")]
        [SwaggerOperation(Summary = "Downgrade Subscription webhook")]
        public async Task<IActionResult> DowngradeSubscriptionWebhook([FromBody] ChangeSubscriptionWebhookDto changeSubscriptionWebhookDto)
        {
            var tenantId = Request.Headers["TenantId"];
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " + changeSubscriptionWebhookDto);
            await _mediator.Send(new DowngradeSubscriptionUseCase(changeSubscriptionWebhookDto));
            return Ok();
        }
        
        //Retrieve AddOn Webhooks
        [HttpPost("addOn")]
        [SwaggerOperation(Summary = "Add On webhook")]
        public async Task<IActionResult> AddOnWebhook([FromBody] AddOnUsageFromWebhookForCreationDto addOnUsageFromWebhookForCreationDto)
        {
            var tenantId = Request.Headers["TenantId"];
            var secret = Request.Headers["Secret"];
            _logger.LogInformation(secret + " " + tenantId + " " );
            var x =await _mediator.Send(new CreateAddOnUsageFromWebhookUseCase(addOnUsageFromWebhookForCreationDto));
            return Ok(x);
        }
    }
}
