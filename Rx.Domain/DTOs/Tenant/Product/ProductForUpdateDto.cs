

using Microsoft.AspNetCore.Http;

namespace Rx.Domain.DTOs.Tenant.Product
{
    public record ProductForUpdateDto(string Name,string Description,IFormFile? LogoImage,string WebhookURL,string RedirectUrl,int FreeTrialDays);
}
