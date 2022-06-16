using Rx.Domain.Enums;

namespace Rx.Domain.DTOs.User;

public record AddUserRequest(string Username,string Email,string Role);