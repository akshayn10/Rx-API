using MediatR;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record RevokeTokenUseCase(string Token):IRequest<bool>;

public class RevokeTokenUseCaseHandler : IRequestHandler<RevokeTokenUseCase, bool>
{
    private readonly IUserService _userService;

    public RevokeTokenUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(RevokeTokenUseCase request, CancellationToken cancellationToken)
    {
        return _userService.RevokeToken(request.Token);
    }
}