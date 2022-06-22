using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record ForgotPasswordUseCase(ForgotPasswordRequest ForgotPasswordRequest,string Origin):IRequest<string>;

public class ForgotPasswordUseCaseHandler : IRequestHandler<ForgotPasswordUseCase,string>
{
    private readonly IUserService _userService;

    public ForgotPasswordUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<string> Handle(ForgotPasswordUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.ForgotPassword(request.ForgotPasswordRequest, request.Origin);
    }
}