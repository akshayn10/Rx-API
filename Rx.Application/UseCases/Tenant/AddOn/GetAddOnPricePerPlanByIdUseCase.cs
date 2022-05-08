using MediatR;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record GetAddOnPricePerPlanByIdUseCase(Guid AddOnId,Guid PlanId, Guid AddOnPricePerPlanId):IRequest<AddOnPricePerPlanDto>;

