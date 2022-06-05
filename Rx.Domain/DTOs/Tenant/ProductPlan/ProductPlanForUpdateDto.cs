namespace Rx.Domain.DTOs.Tenant.ProductPlan;

public record ProductPlanForUpdateDto(string Name,string Description, decimal Price,int Duration, Guid ProductId);