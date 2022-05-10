using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rx.Application.UseCases.Tenant.Webhook;

namespace Rx.API.Controllers.Tenant.Authorization;

public class WebhookVerificationAttribute : Attribute,IAsyncAuthorizationFilter
{
        
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        IMediator? mediator = (IMediator)context.HttpContext.RequestServices.GetService(typeof(IMediator))!;
        
        var webhookSecretForProduct =await mediator.Send(new GetWebhookSecretUseCase(Guid.Parse(context.HttpContext.Request.Headers["ProductId"])));
        if(webhookSecretForProduct != context.HttpContext.Request.Headers["Secret"]) 
        {
            context.Result = new UnauthorizedResult();
        }
        return;
    }
}