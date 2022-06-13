﻿using MediatR;
using Microsoft.Extensions.Primitives;
using Rx.Domain.DTOs.User;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Wrappers;

namespace Rx.Application.UseCases.UserManagement;

public record RegisterUseCase(RegisterRequest RegisterRequest,string Origin) : IRequest<ResponseMessage<string>>;

public class RegisterUseCaseHandler : IRequestHandler<RegisterUseCase, ResponseMessage<string>>
{
    private readonly IUserService _userService;

    public RegisterUseCaseHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ResponseMessage<string>> Handle(RegisterUseCase request, CancellationToken cancellationToken)
    {
        return await _userService.RegisterAsync(request.RegisterRequest, request.Origin);
    }
}