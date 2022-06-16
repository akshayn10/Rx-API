using MediatR;
using Rx.Domain.Entities.Identity;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record GetUserByIdUseCase(string UserId):IRequest<ApplicationUser>;

public class GetUserByIdUseCaseHandler : IRequestHandler<GetUserByIdUseCase, ApplicationUser>
{
    private readonly IUserService _userService;

    public GetUserByIdUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ApplicationUser> Handle(GetUserByIdUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.GetById(request.UserId);
    }
}