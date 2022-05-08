using MediatR;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record AddAddOnUseCase(Guid ProductId,AddOnForCreationDto AddOnForCreationDto):IRequest<AddOnDto>;

public class AddAddOnUseCaseHandler : IRequestHandler<AddAddOnUseCase, AddOnDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public AddAddOnUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<AddOnDto> Handle(AddAddOnUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.AddOnService.CreateAddOn(request.ProductId,request.AddOnForCreationDto);
    }
}