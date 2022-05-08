using MediatR;
using Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;

namespace Rx.Application.UseCases.Tenant.AddOn;

public record AddAddOnPricePerPlanUseCase(Guid AddOnId, Guid PlanId, AddOnPricePerPlanForCreationDto AddOnPricePerPlanForCreationDto):IRequest<AddOnPricePerPlanDto>;
