using MediatR;
using Rx.Domain.DTOs.Tenant.ProductPlan;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.ProductPlan;

public record GetProductPlanByIdUseCase(Guid ProductId,Guid PlanId):IRequest<ProductPlanDto>;

public class GetProductPlanByIdUseCaseHandler:IRequestHandler<GetProductPlanByIdUseCase,ProductPlanDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetProductPlanByIdUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<ProductPlanDto> Handle(GetProductPlanByIdUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductPlanService.GetProductPlanById(request.ProductId, request.PlanId); 
    }
}