namespace Rx.Domain.DTOs.Tenant.Product
{
    public record ProductDto(Guid ProductId,string Name,string Description,string RedirectUrl,string WebhookURL, string WebhookSecret,string LogoURL,int FreeTrialDays);
}
