using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record UpdateUserUseCase(Guid UserId,UpdateUserRequest UpdateUserRequest):IRequest<string>;
public class UpdateUserUseCaseHandler : IRequestHandler<UpdateUserUseCase, string>
{
    private readonly IUserService _userService;

    public UpdateUserUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<string> Handle(UpdateUserUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.EditUserDetails(request.UserId, request.UpdateUserRequest);
    }
}