namespace Rx.Domain.DTOs.Tenant.ProductPlan;

public record ProductPlanForCreationDto(string Name, decimal Price,int Duration,string Description, bool Trial, Guid ProductId);