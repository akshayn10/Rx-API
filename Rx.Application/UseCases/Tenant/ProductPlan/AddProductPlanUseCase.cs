using MediatR;
using Rx.Domain.DTOs.Tenant.Product;
using Rx.Domain.DTOs.Tenant.ProductPlan;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.ProductPlan;

public record AddProductPlanUseCase(ProductPlanForCreationDto PlanForCreationDto):IRequest<ProductPlanDto>;

public class AddProductPlanUseCaseHandler : IRequestHandler<AddProductPlanUseCase, ProductPlanDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public AddProductPlanUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<ProductPlanDto> Handle(AddProductPlanUseCase request, CancellationToken cancellationToken)
    {
        return _tenantServiceManager.ProductPlanService.AddProductPlan(request.PlanForCreationDto);
    }
}