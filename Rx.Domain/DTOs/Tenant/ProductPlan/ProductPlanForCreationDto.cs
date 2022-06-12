namespace Rx.Domain.DTOs.Tenant.ProductPlan;

public record ProductPlanForCreationDto(string Name,string Description, decimal Price,Duration Duration,bool HaveTrial, Guid ProductId);

public record Duration(int Value,string Period);