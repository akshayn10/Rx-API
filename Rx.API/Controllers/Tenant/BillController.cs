using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Rx.API.Controllers.Tenant;

[ApiController]
[Route("api/bill")]
public class BillController:ControllerBase
{
    private readonly IMediator _mediator;

    public BillController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
}