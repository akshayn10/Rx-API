using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.UserManagement;
using Rx.Domain.DTOs.User;

namespace Rx.API.Controllers.UserService;

[ApiController]
[Route("api/user")]
public class UserController:ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
    {
        var response = await _mediator.Send(new LoginUseCase(request));
        return Ok(response);
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        var origin = Request.Headers["origin"];
        var response = await _mediator.Send(new RegisterUseCase(request,origin));
        return Ok(response);
    }
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery]string userId, [FromQuery]string code)
    {
        var origin = Request.Headers["origin"];
        var response =await _mediator.Send(new ConfirmEmailUseCase(userId, code));
        return Ok(response);
    }
}