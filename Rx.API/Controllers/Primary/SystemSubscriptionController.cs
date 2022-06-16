using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Rx.API.Controllers.Primary;

[ApiController]
[Route("api/org/subscription")]
public class SystemSubscriptionController:ControllerBase
{
    private readonly IMediator _mediator;

    public SystemSubscriptionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
}