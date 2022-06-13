using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Rx.Domain.Interfaces.UtcDateTime;
using Rx.Domain.DTOs.Email;
using Rx.Domain.DTOs.User;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Enums;
using Rx.Domain.Exceptions;
using Rx.Domain.Interfaces.Email;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Settings;
using Rx.Domain.Wrappers;

namespace Rx.Infrastructure.Identity.Service;

public class UserService:IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOptions<JwtSecurityTokenSettings> _jwtSettings;
    private readonly IDateTimeService _dateTimeService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService _emailService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JwtSecurityTokenSettings> jwtSettings,
        IDateTimeService dateTimeService,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings;
        _dateTimeService = dateTimeService;
        _signInManager = signInManager;
        _emailService = emailService;
    }
    public async Task<ResponseMessage<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseMessage<string>> RegisterAsync(RegisterRequest request, string origin)
    {
          var user = new ApplicationUser
                    {
                        Email = request.Email,
                        FullName = request.FullName,
                        UserName = request.UserName
                    };
        var userWithSameUserEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameUserEmail != null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            var verificationUri = await SendVerificationEmail(user, origin);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user,
                    new[] {Roles.Owner.ToString(), Roles.Admin.ToString(), Roles.FinanceUser.ToString()});
                var emailRequest = new EmailRequest()
                {
                    To = user.Email,
                    Subject = "Registration Confirmation",
                    Body = $"Please confirm your account by visiting this URL {verificationUri}"

                };
                await _emailService.SendAsync(emailRequest);
                return new ResponseMessage<string>(user.Id,
                    message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
            }
            else
            {
                throw new ApiException($"{result.Errors}");
            }
        }
        else
        {
            throw new ApiException($"Email {request.Email } is already registered.");
        }


    }
    private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "api/account/confirm-email/";
        var _enpointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        //Email Service Call Here
        return verificationUri;
    }

    public async Task<ResponseMessage<string>> ConfirmEmailAsync(string userId, string code)
    {
        throw new NotImplementedException();
    }

    public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseMessage<string>> ResetPassword(ResetPasswordRequest model)
    {
        throw new NotImplementedException();
    }

    public async Task<string> AddRoleAsync(AddRoleModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string jwtToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser> GetById(string id)
    {
        throw new NotImplementedException();
    }
}