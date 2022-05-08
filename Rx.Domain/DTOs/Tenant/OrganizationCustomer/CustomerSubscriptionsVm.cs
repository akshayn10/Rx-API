namespace Rx.Domain.DTOs.Tenant.OrganizationCustomer;

public record CustomerSubscriptionsVm(string subscriptionId,string? product,string? plan,string? createdDate,string? endDate,string status);