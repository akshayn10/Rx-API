using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Wrappers;

namespace Rx.Application.UseCases.UserManagement;

public record ChangePasswordUseCase(ChangePasswordRequest ChangePasswordRequest):IRequest<ResponseMessage<string>>;

public class ChangePasswordUseCaseHandler : IRequestHandler<ChangePasswordUseCase, ResponseMessage<string>>
{
    private readonly IUserService _userService;

    public ChangePasswordUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ResponseMessage<string>> Handle(ChangePasswordUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.ChangePasswordAsync(request.ChangePasswordRequest);
    }
}