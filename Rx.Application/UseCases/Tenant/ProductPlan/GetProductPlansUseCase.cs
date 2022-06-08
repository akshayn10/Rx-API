using MediatR;
using Rx.Domain.DTOs.Tenant.ProductPlan;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.ProductPlan;

public record GetProductPlansUseCase(Guid ProductId):IRequest<IEnumerable<ProductPlanDto>>;

public class GetProductPlansUseCaseHandler:IRequestHandler<GetProductPlansUseCase,IEnumerable<ProductPlanDto>>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public GetProductPlansUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public async Task<IEnumerable<ProductPlanDto>> Handle(GetProductPlansUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.ProductPlanService.GetProductPlans(request.ProductId);
    }
}