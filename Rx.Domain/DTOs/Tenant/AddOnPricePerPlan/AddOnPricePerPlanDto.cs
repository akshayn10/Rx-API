namespace Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;

public record AddOnPricePerPlanDto(Guid AddOnPricePerPlanId, decimal Price, Guid AddOnId, Guid ProductPlanId);
