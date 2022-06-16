namespace Rx.Domain.DTOs.Tenant.AddOnPricePerPlan;

public record AddOnForUpdateDto(string Name,string UnitOfMeasure, decimal Price, string PlanName,Guid AddOnId);
