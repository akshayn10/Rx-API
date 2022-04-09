using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rx.Application.UseCases.Tenant.Webhook;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;
using Service;

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
        
        [HttpPost("subscribe",Name = "RetrieveSubscriptionWebhook") ]
        
        public async Task<IActionResult> RetrieveWebhook()
        {
            var data = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var tenantId = Request.Headers["TenantId"];
            var productId = Guid.Parse(Request.Headers["ProductId"]);
            var secret = Guid.Parse(Request.Headers["Secret"]);
            // var secret = Request.Headers["Secret"];
            // var productSecret =await _mediator.Send(new GetWebhookSecretUseCase(productId));
            // if(productSecret != secret)
            // {
            //     return Unauthorized();
            // }
            
            _logger.LogInformation(secret+" "+tenantId+" " +data);
            return Ok();
        }

    }

    public class WebhookVerificationAttribute : Attribute,IAsyncAuthorizationFilter
    {
        
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            IMediator? mediator = (IMediator)context.HttpContext.RequestServices.GetService(typeof(IMediator))!;
            // if(context.HttpContext.Request.Headers["Secret"]  null)
            // {
            //     context.Result = new UnauthorizedResult();
            //     return;
            // }
            var isValidSecret = Guid.TryParse(context.HttpContext.Request.Headers["Secret"], out _);
            if (!isValidSecret)
            {
                context.Result= new UnauthorizedResult();
                return;
            }
            var productSecret =await mediator.Send(new GetWebhookSecretUseCase(Guid.Parse(context.HttpContext.Request.Headers["ProductId"])));
            if(productSecret != Guid.Parse(context.HttpContext.Request.Headers["Secret"]) )
            {
                context.Result = new UnauthorizedResult();
            }
            return;
        }
    }
}
