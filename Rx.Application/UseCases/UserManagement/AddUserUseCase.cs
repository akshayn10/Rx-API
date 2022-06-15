using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Wrappers;

namespace Rx.Application.UseCases.UserManagement;

public record AddUserUseCase(AddUserRequest AddUserRequest,string Origin):IRequest<ResponseMessage<string>>;

public class AddUserUseCaseHandler:IRequestHandler<AddUserUseCase,ResponseMessage<string>>
{
    private readonly IUserService _userService;

    public AddUserUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ResponseMessage<string>> Handle(AddUserUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.AddUserAsync(request.AddUserRequest,request.Origin);
    }
}