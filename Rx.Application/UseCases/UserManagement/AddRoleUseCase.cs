using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record AddRoleUseCase(AddRoleModel AddRoleModel):IRequest<string>;

public class AddRoleUseCaseHandler:IRequestHandler<AddRoleUseCase,string>
{
    private readonly IUserService _userService;

    public AddRoleUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<string> Handle(AddRoleUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.AddRoleAsync(request.AddRoleModel);
    }
}