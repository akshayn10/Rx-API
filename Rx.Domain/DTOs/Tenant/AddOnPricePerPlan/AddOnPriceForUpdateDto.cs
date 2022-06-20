namespace Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;

public record AddOnPriceForUpdateDto(Guid AddOnPricePerPlanId, decimal Price, Guid AddOnId, Guid ProductPlanId);
