using MediatR;
using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record EditAddOnUseCase(Guid AddOnId,Guid ProductId, AddOnForUpdateDto AddOnForUpdateDto):IRequest<AddOnDto>;

public class EditAddOnUseCaseHandler : IRequestHandler<EditAddOnUseCase, AddOnDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;
   
    public EditAddOnUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }

    public Task<AddOnDto> Handle(EditAddOnUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.AddOnService.UpdateAddOn(request.AddOnId, request.ProductId,request.AddOnForUpdateDto);
    }
}