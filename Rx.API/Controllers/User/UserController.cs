using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rx.Application.UseCases.UserManagement;
using Rx.Domain.DTOs.User;

namespace Rx.API.Controllers.User;

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
        SetRefreshTokenInCookie(response.Data.RefreshToken);
        return Ok(response);
    }
    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequest request)
    {
        var origin = Request.Headers["origin"];
        var response = await _mediator.Send(new RegisterUseCase(request,origin));
        return Ok(response);
    }
    
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery]string userId, [FromQuery]string code)
    {
        var response =await _mediator.Send(new ConfirmEmailUseCase(userId, code));
        return Ok(response);
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
    {
        var origin = Request.Headers["origin"];
        var response = await _mediator.Send(new ForgotPasswordUseCase(model,origin));
        return Ok(response);
    }
    
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
    {
        var response = await _mediator.Send(new ResetPasswordUseCase(model));
        return Ok(response);
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _mediator.Send(new RefreshTokenUseCase(refreshToken));
        if (!string.IsNullOrEmpty(response.RefreshToken))
            SetRefreshTokenInCookie(response.RefreshToken);
        return Ok(response);
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RevokeTokenRequest request)
    {
        var token = request.Token ?? Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token is required" });
        var response = await _mediator.Send(new RevokeTokenUseCase(token));
        return Ok("Logout success");
    }
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var response = await _mediator.Send(new ChangePasswordUseCase(request));
        return Ok(response);
    }
    [HttpPost("add-user")]
    public async Task<IActionResult> AddUser(AddUserRequest request)
    {
        var origin = Request.Headers["origin"];
        var response = await _mediator.Send(new AddUserUseCase(request,origin));
        return Ok(response);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
    {
        // accept token from request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token is required" });

        var response =await _mediator.Send(new RevokeTokenUseCase(token));

        if (response == null)
            return NotFound(new { message = "Token not found" });

        return Ok(new { message = "Token revoked" });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var response = await _mediator.Send(new GetUserByIdUseCase(id));
        return Ok(response);
    }
}