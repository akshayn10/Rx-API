using MediatR;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record GetAddOnPricePerPlanUseCase(Guid AddOnId,Guid PlanId):IRequest<IEnumerable<AddOnPricePerPlanDto>>;
