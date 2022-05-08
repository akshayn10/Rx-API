using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Rx.API.Controllers.Tenant;


[ApiController]
[Route("api/AddOnUsage")]
public class AddUsageController:ControllerBase
{
    private readonly IMediator _mediator;

    public AddUsageController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
}