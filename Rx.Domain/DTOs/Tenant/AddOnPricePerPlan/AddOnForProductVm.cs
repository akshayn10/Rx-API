namespace Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;

public record AddOnForProductVm(string Name,string UnitOfMeasure, decimal Price, string PlanName,Guid AddOnId,Guid AddOnPricePerPlanId,Guid ProductPlanId);
