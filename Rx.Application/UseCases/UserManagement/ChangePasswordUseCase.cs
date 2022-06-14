using MediatR;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;

namespace Rx.Application.UseCases.UserManagement;

public record ChangePasswordUseCase(ChangePasswordRequest ChangePasswordRequest):IRequest<string>;

public class ChangePasswordUseCaseHandler : IRequestHandler<ChangePasswordUseCase, string>
{
    public ChangePasswordUseCaseHandler(IUserService userService)
    {
        
    }

    public async Task<string> Handle(ChangePasswordUseCase request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}