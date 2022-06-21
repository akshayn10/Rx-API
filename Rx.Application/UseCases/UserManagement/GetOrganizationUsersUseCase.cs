using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record GetOrganizationUsersUseCase(Guid OrganizationId):IRequest<IEnumerable<UserVm>>;

public class GetOrganizationUsersUseCaseHandler : IRequestHandler<GetOrganizationUsersUseCase, IEnumerable<UserVm>>
{
    private readonly IUserService _userService;

    public GetOrganizationUsersUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<IEnumerable<UserVm>> Handle(GetOrganizationUsersUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.GetUsersForOrganization(request.OrganizationId);
    }
}