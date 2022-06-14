using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Rx.Domain.DTOs.User;


public record RegisterRequest(string FullName, string Email, IFormFile? ProfileImage, string UserName, string Password, string ConfirmPassword);