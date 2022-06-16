using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Wrappers;

namespace Rx.Application.UseCases.UserManagement;

public record LoginUseCase(AuthenticationRequest AuthenticationRequest):IRequest<ResponseMessage<AuthenticationResponse>>;

public class LoginUseCaseHandler : IRequestHandler<LoginUseCase, ResponseMessage<AuthenticationResponse>>
{
    private readonly IUserService _userService;

    public LoginUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<ResponseMessage<AuthenticationResponse>> Handle(LoginUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.AuthenticateAsync(request.AuthenticationRequest);
    }
}
