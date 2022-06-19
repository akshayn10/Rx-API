using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.Tenant.Report;
using Rx.Application.UseCases.Tenant.Subscription;

namespace Rx.API.Controllers.Tenant;

[ApiController]
[Route("api/report")]
public class ReportController:ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetSubscriptionStats()
    {
        var res = await _mediator.Send(new GetSubscriptionStatUseCase());
        return Ok(res);
    }
}