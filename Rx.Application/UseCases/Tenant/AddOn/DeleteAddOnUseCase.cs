using MediatR;
using Rx.Application.UseCases.Tenant.AddOn;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record DeleteAddOnUseCase(Guid addOnId) : IRequest<string>;

public class DeleteAddOnUseCaseHandler : IRequestHandler<DeleteAddOnUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;
        
    public DeleteAddOnUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
        
    public async Task<string> Handle(DeleteAddOnUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.AddOnService.DeleteAddOn(request.addOnId);

    }
            

}