using System.ComponentModel.DataAnnotations;

namespace Rx.Domain.DTOs.User;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}