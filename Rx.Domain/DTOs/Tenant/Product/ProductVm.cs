namespace Rx.Domain.DTOs.Tenant.Product;

public record ProductVm(string ProductId,string Name,int PlanCount,int AddOnCount,string WebhookURL,string LogoURL);