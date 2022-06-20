using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Rx.API.Controllers.Primary;

[ApiController]
[Route("api/org/plan")]
public class SubscriptionPlanController:ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionPlanController(IMediator mediator)
    {
        _mediator = mediator;
    }
}