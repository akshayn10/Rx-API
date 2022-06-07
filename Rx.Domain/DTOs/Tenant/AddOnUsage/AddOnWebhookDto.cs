namespace Rx.Domain.DTOs.Tenant.AddOnUsage;

public record AddOnWebhookDto(Guid SenderAddOnWebhookId,Guid AddOnId,Guid SubscriptionId,int Unit,Guid OrganizationCustomerId,DateTime RetrievedDateTime);