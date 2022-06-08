namespace Rx.Domain.DTOs.Tenant.ProductPlan;

public record ProductPlanDto(Guid PlanId,string Name,string Description, decimal Price,int Duration,bool HaveTrial, Guid ProductId);

