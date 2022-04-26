namespace Rx.Domain.DTOs.Tenant.Product
{
    public record ProductDto(Guid productId,string Name,string Description,string WebhookURL,string WebhookSecret,string LogoURL,int FreeTrialDays);
}
