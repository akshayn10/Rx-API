using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record ForgotPasswordUseCase(ForgotPasswordRequest ForgotPasswordRequest,string Origin):IRequest;

public class ForgotPasswordUseCaseHandler : IRequestHandler<ForgotPasswordUseCase>
{
    private readonly IUserService _userService;

    public ForgotPasswordUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<Unit> Handle(ForgotPasswordUseCase request, CancellationToken cancellationToken)
    {
        await _userService.ForgotPassword(request.ForgotPasswordRequest, request.Origin);
        return  Unit.Value;
    }
}