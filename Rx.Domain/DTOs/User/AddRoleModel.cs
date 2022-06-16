using System.ComponentModel.DataAnnotations;

namespace Rx.Domain.DTOs.User;

public class AddRoleModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Role { get; set; }
}