using Rx.Domain.DTOs.User;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Wrappers;

namespace Rx.Domain.Interfaces.Identity;

public interface IUserService
{
    Task<ResponseMessage<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
    Task<ResponseMessage<string>> RegisterAsync(RegisterRequest request, string origin);
    Task<ResponseMessage<string>> ConfirmEmailAsync(string userId, string code);
    Task ForgotPassword(ForgotPasswordRequest model, string origin);
    Task<ResponseMessage<string>> ResetPassword(ResetPasswordRequest model);
    Task<string> AddRoleAsync(AddRoleModel model);
    Task<AuthenticationResponse> RefreshTokenAsync(string token);
    Task<ApplicationUser> GetById(string id);
    bool RevokeToken(string token);
    Task<string> AddUserAsync(AddUserRequest request,string origin);
}