using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record RefreshTokenUseCase(string Token):IRequest<AuthenticationResponse>;

public class RefreshTokenUseCaseHandler : IRequestHandler<RefreshTokenUseCase, AuthenticationResponse>
{
    private readonly IUserService _userService;

    public RefreshTokenUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<AuthenticationResponse> Handle(RefreshTokenUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.RefreshTokenAsync(request.Token);
    }
}