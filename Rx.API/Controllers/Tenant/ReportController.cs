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
    [HttpGet("subscription-summary")]
    public async Task<IActionResult> GetSubscriptionStats()
    {
        var res = await _mediator.Send(new GetSubscriptionStatUseCase());
        return Ok(res);
    }
    [HttpGet("subscription-activation")]
    public async Task<IActionResult> GetSubscriptionActivation()
    {
        var res = await _mediator.Send(new GetSubscriptionActivationUseCase());
        return Ok(res);
    }
    [HttpGet("sales-by-plan")]
    public async Task<IActionResult> GetSalesByPlan()
    {
        var res = await _mediator.Send(new GetSalesByPlanUseCase());
        return Ok(res);
    }
    [HttpGet("sales-by-addOn")]
    public async Task<IActionResult> GetSalesByAddOn()
    {
        var res = await _mediator.Send(new GetSalesByAddonUseCase());
        return Ok(res);
    }
    [HttpGet("upgrade")]
    public async Task<IActionResult> GetUpgradeCount()
    {
        var res = await _mediator.Send(new GetUpgradeCountUseCase());
        return Ok(res);
    }
    [HttpGet("downgrade")]
    public async Task<IActionResult> GetDowngradeCount()
    {
        var res = await _mediator.Send(new GetDowngradeCountUseCase());
        return Ok(res);
    }
    [HttpGet("unsubscribe")]
    public async Task<IActionResult> GetUnsubscriptionCount()
    {
        var res = await _mediator.Send(new GetUnsubscriptionUseCase());
        return Ok(res);
    }
    
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue()
    {
        var res = await _mediator.Send(new GetRevenueStatsUseCase());
        return Ok(res);
    }
    
}