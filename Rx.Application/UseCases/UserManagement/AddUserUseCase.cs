using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record AddUserUseCase(AddUserRequest AddUserRequest,string Origin):IRequest<string>;

public class AddUserUseCaseHandler:IRequestHandler<AddUserUseCase,string>
{
    private readonly IUserService _userService;

    public AddUserUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(AddUserUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.AddUserAsync(request.AddUserRequest,request.Origin);
    }
}