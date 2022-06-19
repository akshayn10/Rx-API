namespace Rx.Domain.DTOs.Tenant.Dashboard;

public record DashboardStats(
    int TotalCustomers,
    decimal TotalRevenue,
    int TotalSubscriptions,
    int TotalProducts
);