namespace Rx.Domain.DTOs.Tenant.ProductPlan;

public record ProductPlanForCreationDto(string Name,string Description, decimal Price,int Duration,bool HaveTrial, Guid ProductId);