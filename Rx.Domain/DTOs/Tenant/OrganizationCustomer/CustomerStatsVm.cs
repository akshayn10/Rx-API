namespace Rx.Domain.DTOs.Tenant.OrganizationCustomer;

public record CustomerStatsVm(int TotalCustomers,
    int ActiveCustomers,
    int InactiveCustomers,
    int NewCustomerCountForMonth);