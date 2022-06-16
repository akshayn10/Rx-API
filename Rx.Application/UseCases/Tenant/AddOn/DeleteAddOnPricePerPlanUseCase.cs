using MediatR;
using Rx.Application.UseCases.Tenant.ProductPlan;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record DeleteAddOnPricePerPlanUseCase(Guid addOnId) : IRequest<string>;

    public class DeleteAddOnPricePerPlanUseCaseHandler : IRequestHandler<DeleteAddOnPricePerPlanUseCase, string>
    {
        private readonly ITenantServiceManager _tenantServiceManager;
        
        public DeleteAddOnPricePerPlanUseCaseHandler(ITenantServiceManager tenantServiceManager)
        {
            _tenantServiceManager = tenantServiceManager;
        }
        
        public async Task<string> Handle(DeleteAddOnPricePerPlanUseCase request, CancellationToken cancellationToken)
        {
            return await _tenantServiceManager.AddOnService.DeleteAddOn(request.addOnId);

            }
            

}