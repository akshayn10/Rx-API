using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Primary.SubscriptionPlan;

namespace Rx.API.Controllers.Primary;

[ApiController]
[Route("api/organization/plan")]
public class SubscriptionPlanController:ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionPlanController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllSubscriptionPlanUseCase());
        return Ok(result);
    }
}