using Microsoft.AspNetCore.Http;

namespace Rx.Domain.DTOs.User;

public record UpdateUserRequest(string FullName, string Email, IFormFile? ProfileImage, string UserName);