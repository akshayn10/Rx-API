namespace Rx.Domain.DTOs.Tenant.Product;

public record ProductVm(string ProductId,string Name,int PlanCount,int AddOnCount,string RedirectURL,string LogoURL);