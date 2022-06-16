using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Wrappers;

namespace Rx.Application.UseCases.UserManagement;

public record ResetPasswordUseCase(ResetPasswordRequest ResetPasswordRequest):IRequest<ResponseMessage<string>>;

public class ResetPasswordUseCaseHandler : IRequestHandler<ResetPasswordUseCase, ResponseMessage<string>>
{
    private readonly IUserService _userService;

    public ResetPasswordUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ResponseMessage<string>> Handle(ResetPasswordUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.ResetPassword(request.ResetPasswordRequest);
    }
}