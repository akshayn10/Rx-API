using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Rx.Application.UseCases.Primary.Organization;

namespace Rx.API.Middleware;

public class SubscriptionVerificationAttribute: Attribute,IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        IMediator mediator = (IMediator)context.HttpContext.RequestServices.GetService(typeof(IMediator))!;
        var organizationId = context.HttpContext.Request.Headers["OrganizationId"];
        if (StringValues.IsNullOrEmpty(organizationId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        var subscription = await mediator.Send(new GetOrganizationSubscriptionStatusUseCase(Guid.Parse(organizationId)));

        if (!subscription)
        {
            context.Result = new UnauthorizedResult();

        }
        return;
    }
}

