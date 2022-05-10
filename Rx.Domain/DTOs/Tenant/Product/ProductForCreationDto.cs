namespace Rx.Domain.DTOs.Tenant.Product
{
    public record ProductForCreationDto(string Name,string Description,string WebhookURL,string LogoURL,int FreeTrialDays);
}
