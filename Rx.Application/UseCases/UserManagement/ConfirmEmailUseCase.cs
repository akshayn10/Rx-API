using MediatR;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Wrappers;

namespace Rx.Application.UseCases.UserManagement;

public record ConfirmEmailUseCase(string UserId,string Code):IRequest<ResponseMessage<string>>;

public class ConfirmEmailUseCaseHandler : IRequestHandler<ConfirmEmailUseCase, ResponseMessage<string>>
{
    private readonly IUserService _userService;

    public ConfirmEmailUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ResponseMessage<string>> Handle(ConfirmEmailUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.ConfirmEmailAsync(request.UserId, request.Code);
    }
}
