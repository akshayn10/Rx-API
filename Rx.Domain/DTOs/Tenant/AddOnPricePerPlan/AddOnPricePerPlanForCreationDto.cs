namespace Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;

public record AddOnPricePerPlanForCreationDto(decimal Price, Guid AddOnId, Guid ProductPlanId);