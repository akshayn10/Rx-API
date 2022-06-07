using Rx.Domain.DTOs.Tenant.AddOn;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.Entities.Tenant;

namespace Rx.Domain.Interfaces.Tenant;

public interface IAddOnUsageService
{
    Task<AddOnUsageDto> CreateAddOnUsage(Guid subscriptionId,Guid addOnId,AddOnUsageForCreationDto addOnUsageForCreationDto);
    Task<string> CreateAddOnUsageFromWebhook(AddOnWebhook addOnWebhook);
    Task<string> ActivateAddOnUsageAfterPayment(string webhookId,long amount);
}