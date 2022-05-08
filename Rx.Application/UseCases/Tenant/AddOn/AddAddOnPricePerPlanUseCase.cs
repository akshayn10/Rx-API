using MediatR;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record AddAddOnPricePerPlanUseCase(Guid AddOnId, Guid PlanId, AddOnPricePerPlanForCreationDto AddOnPricePerPlanForCreationDto):IRequest<AddOnPricePerPlanDto>;

public class AddAddOnPricePerPlanUseCaseHandler : IRequestHandler<AddAddOnPricePerPlanUseCase, AddOnPricePerPlanDto>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public AddAddOnPricePerPlanUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;
    }
    public Task<AddOnPricePerPlanDto> Handle(AddAddOnPricePerPlanUseCase request, CancellationToken cancellationToken)
    {
        var addOnPricePlan = _tenantServiceManager.AddOnService.CreateAddOnPricePerPlan(request.AddOnId, request.PlanId, request.AddOnPricePerPlanForCreationDto);
        return addOnPricePlan;
    }
}
