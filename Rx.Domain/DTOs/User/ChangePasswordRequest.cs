namespace Rx.Domain.DTOs.User;

public record ChangePasswordRequest(string Email, string OldPassword, string NewPassword);