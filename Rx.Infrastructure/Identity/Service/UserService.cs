using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rx.Domain.Interfaces.UtcDateTime;
using Rx.Domain.DTOs.Email;
using Rx.Domain.DTOs.User;
using Rx.Domain.Enums;
using Rx.Domain.Exceptions;
using Rx.Domain.Interfaces.Blob;
using Rx.Domain.Interfaces.Email;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Settings;
using Rx.Domain.Wrappers;
using Rx.Infrastructure.Identity.Contexts;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.Entities.Identity;

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
    private readonly ILogger<IUserService> _logger;
    private readonly IBlobStorage _blobStorage;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JwtSecurityTokenSettings> jwtSettings,
        IDateTimeService dateTimeService,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService,
        IdentityContext identityContext,
        ILogger<IUserService> logger,
        IBlobStorage blobStorage,
        IBackgroundJobClient backgroundJobClient
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings.Value;
        _dateTimeService = dateTimeService;
        _signInManager = signInManager;
        _emailService = emailService;
        _identityContext = identityContext;
        _logger = logger;
        _blobStorage = blobStorage;
        _backgroundJobClient = backgroundJobClient;
    }
    public async Task<ResponseMessage<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new ApiException($"No User Registered with {request.Email}.");
        }
        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            throw new ApiException($"Invalid Credentials for '{request.Email}'.");
        }
        if (!user.EmailConfirmed)
        {
            throw new ApiException($"User Not Confirmed for '{request.Email}'.");
        }
        
        
        //Generate JWT Token
        var jwtSecurityToken = await GenerateJwtToken(user);
        
        var response = new AuthenticationResponse
        {
            Id = user.Id,
            JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Email = user.Email,
            UserName = user.UserName,
            ProfileUrl = user.ProfileUrl,
            OrganizationId = user.OrganizationId!=null ? user.OrganizationId.ToString() : null
        };
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = rolesList.ToList();
        response.IsVerified = user.EmailConfirmed;
        response.IsAuthenticated = true;
        var refreshToken = GenerateRefreshToken();
        response.RefreshToken = refreshToken.Token;
        
        user.RefreshTokens.Add(refreshToken);
        _identityContext.Update(user);
        await _identityContext.SaveChangesAsync();
        
        return new ResponseMessage<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
    }
    public async Task<ResponseMessage<string>> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new ApiException($"No User Registered with {request.Email}.");
        }
        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if(result.Succeeded)
        {
            var emailRequest = new EmailRequest()
            {
                To = user.Email,
                Subject = "Password Change Successful",
                Body = $"Password Changed Successfully for {user.UserName} at {_dateTimeService.NowUtc}",

            };
            _backgroundJobClient.Enqueue(()=>_emailService.SendAsync(emailRequest));
            // await _emailService.SendAsync(emailRequest);
            return new ResponseMessage<string>(request.Email, message: $"Password Reset Successful.");
        }
        else
        {
            throw new ApiException($"Error occured while password reset.");
        }
        
    }

    public async Task<string> UpdateUserAsync(string userId, Guid organizationId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new ApiException($"No User Registered with {userId}.");
        }
        user.OrganizationId = organizationId;
        _identityContext.Update(user);
        await _identityContext.SaveChangesAsync();
        return "User Updated Successfully";
    }

    public async Task<string> EditUserDetails(string userId, UpdateUserRequest updateUserRequest)
    {
        var user =await _identityContext.Users.FindAsync(userId);
        if (user == null)
        {
            throw new NullReferenceException($"No User Registered with {userId}.");
        }
        string? profileUrl = null;
        if (updateUserRequest.ProfileImage != null)
        {
            var fileName = string.Empty;
            _logger.LogInformation("Upload Started");
            var profileImage = updateUserRequest.ProfileImage;
            if (profileImage.Length > 0)
            {
                await using var fileStream = new FileStream(profileImage.FileName, FileMode.Create);
                _logger.LogInformation("file found");
                await profileImage.CopyToAsync(fileStream);
                fileName = fileStream.Name;
            }

            var stream = File.OpenRead(profileImage.FileName);
            profileUrl = await _blobStorage.UploadProfile(stream);
            _logger.LogInformation("Upload Completed");
            stream.Close();
            File.Delete(fileName);
        }
        user.FullName = updateUserRequest.FullName;
        user.Email = updateUserRequest.Email;
        user.UserName = updateUserRequest.UserName;
        if (profileUrl != null)
        {
            user.ProfileUrl = profileUrl;

        }
        await _identityContext.SaveChangesAsync();
        return "User Updated Successfully";
    }

    public async Task<IEnumerable<UserVm>> GetUsersForOrganization(Guid organizationId)
    {
        var users = await _identityContext.Users.Where(x => x.OrganizationId == organizationId).ToListAsync();


        var userVms = new List<UserVm>();
        foreach (ApplicationUser user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            string role;
            if(roles.Contains("Owner"))
            {
                role = "Owner";
            }
            else if(roles.Contains("Admin"))
            {
                role = "Admin";
            }
            else 
            {
                role = "Finance User";
            }
            var appuser = new UserVm(
                Username:user.UserName,
                Email:user.Email,
                Role:role,
                ProfileUrl:user.ProfileUrl
            );
            userVms.Add(appuser);
        }

        return userVms;

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
        string? profileUrl = null;
        if (request.ProfileImage != null)
        {
            var fileName = string.Empty;
            _logger.LogInformation("Upload Started");
            var profileImage = request.ProfileImage;
            if (profileImage.Length > 0)
            {
                await using var fileStream = new FileStream(profileImage.FileName, FileMode.Create);
                _logger.LogInformation("file found");
                await profileImage.CopyToAsync(fileStream);
                fileName = fileStream.Name;
            }

            var stream = File.OpenRead(profileImage.FileName);
            profileUrl = await _blobStorage.UploadProfile(stream);
            _logger.LogInformation("Upload Completed");
            stream.Close();
            File.Delete(fileName);
        }

        var user = new ApplicationUser
                    {
                        Email = request.Email,
                        FullName = request.FullName,
                        UserName = request.UserName,
                        ProfileUrl = profileUrl
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
                    Body = $"Please confirm your user by visiting this URL {verificationUri}"

                };
                _backgroundJobClient.Enqueue(()=>_emailService.SendAsync(emailRequest));
                // await _emailService.SendAsync(emailRequest);
                return new ResponseMessage<string>(user.Id,
                    message: $"User Registered. Please confirm your user by visiting this URL {verificationUri}");
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
    public async Task<ResponseMessage<string>> AddUserAsync(AddUserRequest request,string origin)
    {
        var oneTimePassword = string.Concat("Rx1", Guid.NewGuid().ToString("N").Substring(0, 9));
        var user = new ApplicationUser
        {
            Email = request.Email,
            FullName = request.Username,
            UserName = request.Username,
            EmailConfirmed = true,
            OrganizationId = Guid.Parse(request.OrganizationId) 
        };
        var userWithSameUserEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameUserEmail == null)
        {
            var result = await _userManager.CreateAsync(user, oneTimePassword);
            if (result.Succeeded)
            {
                if (request.Role == "Admin")
                {
                    await _userManager.AddToRolesAsync(user,
                        new[] {Roles.Admin.ToString(), Roles.FinanceUser.ToString()});
                }
                else if (request.Role == "FinanceUser")
                {
                    await _userManager.AddToRolesAsync(user,
                        new[] {Roles.FinanceUser.ToString()});
                }

                var emailRequest = new EmailRequest()
                {
                    To = user.Email,
                    Subject = $"You have been added to the system as {request.Role}",
                    Body = $"Please Login to the system using Onetime password : {oneTimePassword} \n Login Page: {origin}/auth/login"

                };
                _backgroundJobClient.Enqueue(()=>_emailService.SendAsync(emailRequest));
                // await _emailService.SendAsync(emailRequest);
                return new ResponseMessage<string>(request.Email,
                    message: $"User Added. Please Login to the system using Onetime password : {oneTimePassword} \n Login Page: {origin}/auth/login");
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
            return new ResponseMessage<string>(user.Id, message: $"user Confirmed for {user.Email}. You can now use the /api/user/authenticate endpoint.");
        }
        else
        {
            throw new ApiException($"An error occured while confirming {user.Email}.");
        }
    }

    public async Task<string> ForgotPassword(ForgotPasswordRequest model, string origin)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return "User Not found for the given email";

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var route = "api/user/reset-password/";
        var endPointUri = new Uri(string.Concat($"{origin}/", route));
        var emailRequest = new EmailRequest()
        {
            Body = $"You reset token is - {code}",
            To = model.Email,
            Subject = "Reset Password",
        };
        _backgroundJobClient.Enqueue(()=>_emailService.SendAsync(emailRequest));
        // await _emailService.SendAsync(emailRequest);
        return "Check your mail inbox";
    }

    public async Task<ResponseMessage<string>> ResetPassword(ResetPasswordRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) throw new ApiException($"No user Registered with {model.Email}.");
        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if(result.Succeeded)
        {
            var emailRequest = new EmailRequest()
            {
                To = user.Email,
                Subject = "Password Reset Successful",
                Body = $"Password Reset Successful for {user.UserName} at {_dateTimeService.NowUtc}",

            };
            _backgroundJobClient.Enqueue(()=>_emailService.SendAsync(emailRequest));
            // await _emailService.SendAsync(emailRequest);
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
            return $"No User Registered with {model.Email}.";
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
        
        // //Revoke Current Refresh Token
        // refreshToken.Revoked = DateTime.UtcNow;
        //
        // //Generate new Refresh Token and save to Database
        // var newRefreshToken = GenerateRefreshToken();
        // user.RefreshTokens.Add(newRefreshToken);
        // _identityContext.Update(user);
        // await _identityContext.SaveChangesAsync();
        
        //Generates new jwt
        authenticationResponse.IsAuthenticated = true;
        authenticationResponse.IsVerified = true;
        authenticationResponse.Id = user.Id;
        authenticationResponse.ProfileUrl = user.ProfileUrl;
        authenticationResponse.OrganizationId = user.OrganizationId.ToString();
        JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);
        authenticationResponse.JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authenticationResponse.Email = user.Email;
        authenticationResponse.UserName = user.UserName;
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        authenticationResponse.Roles = rolesList.ToList();
        authenticationResponse.RefreshToken = token;
        authenticationResponse.RefreshTokenExpiration = refreshToken.Expires;
        return authenticationResponse;
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

    }


    public async Task<ApplicationUser> GetById(string id)
    {
        var user = await _identityContext.Users.FindAsync(id);
        var x = new ApplicationUser
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            ProfileUrl = user.ProfileUrl,
            UserName = user.UserName
        };
        return x;

    }
}