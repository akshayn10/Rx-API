using MediatR;
using Rx.Domain.DTOs.Tenant.ProductPlan;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.ProductPlan;

public record EditProductPlanUseCase(Guid ProductId,Guid PlanId,ProductPlanForUpdateDto PlanForUpdateDto):IRequest<ProductPlanDto>;

public class EditProductPlanUseCaseHandler : IRequestHandler<EditProductPlanUseCase, ProductPlanDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;
    
    public EditProductPlanUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    
    public Task<ProductPlanDto> Handle(EditProductPlanUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductPlanService.UpdateProductPlan(request.ProductId,request.PlanId,request.PlanForUpdateDto);
    }




}