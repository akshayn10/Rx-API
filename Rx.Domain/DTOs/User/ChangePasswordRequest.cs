namespace Rx.Domain.DTOs.User;

public record ChangePasswordRequest(string UserId, string OldPassword, string NewPassword);