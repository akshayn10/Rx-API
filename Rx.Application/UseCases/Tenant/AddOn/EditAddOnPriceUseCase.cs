using MediatR;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;
using Rx.Domain.Interfaces;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record EditAddOnPriceUseCase(Guid addOnPricePerPlanId, AddOnPriceForUpdateDto AddOnPriceForUpdateDto):IRequest<AddOnPricePerPlanDto>;

public class EditAddOnPriceUseCaseHandler : IRequestHandler<EditAddOnPriceUseCase, AddOnPricePerPlanDto>
{
   private readonly ITenantServiceManager _tenantServiceManager;
   
   public EditAddOnPriceUseCaseHandler(ITenantServiceManager tenantServiceManager)
   {
      _tenantServiceManager = tenantServiceManager;
   }
   
   public Task<AddOnPricePerPlanDto> Handle(EditAddOnPriceUseCase request, CancellationToken cancellationToken)
   {
      return _tenantServiceManager.AddOnService.UpdateAddOnPrice(request.addOnPricePerPlanId, request.AddOnPriceForUpdateDto);
   }
   
}