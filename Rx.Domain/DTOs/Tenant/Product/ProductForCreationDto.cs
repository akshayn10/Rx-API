namespace Rx.Domain.DTOs.Tenant.Product
{
    public record ProductForCreationDto(string Name,string Description,string LogoURL,string WebhookURL,string RedirectUrl,string WebhookSecret,int FreeTrialDays);
}
