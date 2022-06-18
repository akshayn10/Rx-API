using MediatR;
using Rx.Application.UseCases.Tenant.AddOn;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record DeleteAddOnPricePerPlanUseCase(Guid addOnPricePerPlanId) : IRequest<string>;

    public class DeleteAddOnPricePerPlanUseCaseHandler : IRequestHandler<DeleteAddOnPricePerPlanUseCase, string>
    {
        private readonly ITenantServiceManager _tenantServiceManager;
        
        public DeleteAddOnPricePerPlanUseCaseHandler(ITenantServiceManager tenantServiceManager)
        {
            _tenantServiceManager = tenantServiceManager;
        }
        
        public async Task<string> Handle(DeleteAddOnPricePerPlanUseCase request, CancellationToken cancellationToken)
        {
            return await _tenantServiceManager.AddOnService.DeleteAddOnPrice(request.addOnPricePerPlanId);

            }
            

}