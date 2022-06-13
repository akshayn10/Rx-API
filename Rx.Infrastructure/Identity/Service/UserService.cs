using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rx.Domain.Interfaces.UtcDateTime;
using Rx.Domain.DTOs.Email;
using Rx.Domain.DTOs.User;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Enums;
using Rx.Domain.Exceptions;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Email;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Settings;
using Rx.Domain.Wrappers;
using Rx.Infrastructure.Identity.Contexts;

namespace Rx.Infrastructure.Identity.Service;

public class UserService:IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtSecurityTokenSettings _jwtSettings;
    private readonly IDateTimeService _dateTimeService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IdentityContext _identityContext;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JwtSecurityTokenSettings> jwtSettings,
        IDateTimeService dateTimeService,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService,
        IdentityContext identityContext
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings.Value;
        _dateTimeService = dateTimeService;
        _signInManager = signInManager;
        _emailService = emailService;
        _identityContext = identityContext;
    }
    public async Task<ResponseMessage<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new ApiException($"No Accounts Registered with {request.Email}.");
        }
        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            throw new ApiException($"Invalid Credentials for '{request.Email}'.");
        }
        if (!user.EmailConfirmed)
        {
            throw new ApiException($"Account Not Confirmed for '{request.Email}'.");
        }
        JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);
        AuthenticationResponse response = new AuthenticationResponse();
        response.Id = user.Id;
        response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        response.Email = user.Email;
        response.UserName = user.UserName;
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = rolesList.ToList();
        response.IsVerified = user.EmailConfirmed;
        var refreshToken = GenerateRefreshToken();
        response.RefreshToken = refreshToken.Token;
        return new ResponseMessage<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
    }
    private RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };
    }
    private string RandomTokenString()
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        // convert random bytes to hex string
        return BitConverter.ToString(randomBytes).Replace("-", "");
    }
    private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }


        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
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
        if (userWithSameUserEmail == null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);
            var verificationUri = await VerificationUri(user, origin);
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
    private async Task<string> VerificationUri(ApplicationUser user, string origin)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "api/user/confirm-email/";
        var endPointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(endPointUri.ToString(), "userId", user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        return verificationUri;
    }

    public async Task<ResponseMessage<string>> ConfirmEmailAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if(result.Succeeded)
        {
            return new ResponseMessage<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
        }
        else
        {
            throw new ApiException($"An error occured while confirming {user.Email}.");
        }
    }

    public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return;

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var route = "api/account/reset-password/";
        var endPointUri = new Uri(string.Concat($"{origin}/", route));
        var emailRequest = new EmailRequest()
        {
            Body = $"You reset token is - {code}",
            To = model.Email,
            Subject = "Reset Password",
        };
        await _emailService.SendAsync(emailRequest);
    }

    public async Task<ResponseMessage<string>> ResetPassword(ResetPasswordRequest model)
    {
        var account = await _userManager.FindByEmailAsync(model.Email);
        if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
        var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
        if(result.Succeeded)
        {
            return new ResponseMessage<string>(model.Email, message: $"Password Reset Successful.");
        }
        else
        {
            throw new ApiException($"Error occured while password reset.");
        }
    }

    public async Task<string> AddRoleAsync(AddRoleModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return $"No Accounts Registered with {model.Email}.";
        }
        if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var roleExists = Enum.GetNames(typeof(Roles)).Any(x => x.ToLower() == model.Role.ToLower());
            if (roleExists)
            {
                var validRole = Enum.GetValues(typeof(Roles)).Cast<Roles>().FirstOrDefault(x => x.ToString().ToLower() == model.Role.ToLower());
                await _userManager.AddToRoleAsync(user, validRole.ToString());
                return $"Added {model.Role} to user {model.Email}.";
            }
            return $"Role {model.Role} not found.";
        }
        return $"Incorrect Credentials for user {user.Email}.";
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string token)
    {
        var authenticationResponse = new AuthenticationResponse();
        var user = _identityContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
        
        if (user == null)
        {
            authenticationResponse.IsAuthenticated = false;
            authenticationResponse.Message = $"Token did not match any users.";
            return authenticationResponse;
        }
        
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
        
        if (!refreshToken.IsActive)
        {
            authenticationResponse.IsAuthenticated = false;
            authenticationResponse.Message = $"Token Not Active";
            return authenticationResponse;
        }
        
        //Revoke Current Refresh Token
        refreshToken.Revoked = DateTime.UtcNow;
        
        //Generate new Refresh Token and save to Database
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        _identityContext.Update(user);
        await _identityContext.SaveChangesAsync();
        
        
        //Generates new jwt
        authenticationResponse.IsAuthenticated = true;
        JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);
        authenticationResponse.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authenticationResponse.Email = user.Email;
        authenticationResponse.UserName = user.UserName;
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        authenticationResponse.Roles = rolesList.ToList();
        authenticationResponse.RefreshToken = newRefreshToken.Token;
        authenticationResponse.RefreshTokenExpiration = newRefreshToken.Expires;
        return authenticationResponse;
        throw new NotImplementedException();
    }
    
    public bool RevokeToken(string token)
    {
        var user = _identityContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
        
        // return false if no user found with token
        if (user == null) return false;
        
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
        
        // return false if token is not active
        if (!refreshToken.IsActive) return false;
        
        // revoke token and save
        refreshToken.Revoked = DateTime.UtcNow;
        _identityContext.Update(user);
        _identityContext.SaveChanges();
        
        return true;
        throw new NotImplementedException();

    }


    public async Task<ApplicationUser> GetById(string id)
    {
        var user =await _userManager.FindByIdAsync(id);
        return user;
    }
}