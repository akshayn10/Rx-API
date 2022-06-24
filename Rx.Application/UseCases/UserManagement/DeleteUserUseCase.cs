using MediatR;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record DeleteUserUseCase(string Email):IRequest<string>;

public class DeleteUserUserCaseHandler : IRequestHandler<DeleteUserUseCase, string>
{
    private readonly IUserService _userService;

    public DeleteUserUserCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<string> Handle(DeleteUserUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.DeleteUserAsync(request.Email);
    }
}