using MediatR;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.ProductPlan;

public record DeleteProductPlanUseCase(Guid ProductId,Guid PlanId):IRequest<string>;

public class DeleteProductPlanUseCaseHandler:IRequestHandler<DeleteProductPlanUseCase,string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public DeleteProductPlanUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }

    public async Task<string> Handle(DeleteProductPlanUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.ProductPlanService.DeleteProductPlan(request.ProductId,request.PlanId);
  
    }
}