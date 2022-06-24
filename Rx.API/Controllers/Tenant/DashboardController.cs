using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rx.API.Middleware;
using Rx.Application.UseCases.Tenant.Dashboard;
using Rx.Application.UseCases.Tenant.Report;

namespace Rx.API.Controllers.Tenant;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "FinanceUser")]
[SubscriptionVerification]
public class DashboardController:ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("customersForProducts")]
    public async Task<IActionResult> GetCustomersCountForProducts()
    {
        var result = await _mediator.Send(new GetCustomersCountForProductsUseCase());
        return Ok(result);
    }
    [HttpGet("subscriptionsStats")]
    public async Task<IActionResult> GetSubscriptionsStats()
    {
        var result = await _mediator.Send(new GetSubscriptionStatUseCase());
        return Ok(result);
    }
    [HttpGet("revenueStats")]
    public async Task<IActionResult> GetRevenueStats()
    {
        var result = await _mediator.Send(new GetRevenueStatUseCase());
        return Ok(result);
    }
    [HttpGet("dashboardStats")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var result = await _mediator.Send(new GetDashboardStatUseCase());
        return Ok(result);
    }
    [HttpGet("tableStats")]
    public async Task<IActionResult> GetTableStats()
    {
        var result = await _mediator.Send(new GetTableStatUseCase());
        return Ok(result);
    }

}