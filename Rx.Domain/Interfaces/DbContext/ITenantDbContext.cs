using Microsoft.EntityFrameworkCore;
using Rx.Domain.Entities.Tenant;

namespace Rx.Domain.Interfaces.DbContext;

public interface ITenantDbContext
{
    DbSet<OrganizationCustomer>? OrganizationCustomers { get; set; }
    DbSet<Product>? Products { get; set; }
    DbSet<ProductPlan>? ProductPlans { get; set; }
    DbSet<Subscription>? Subscriptions { get; set; }
    DbSet<AddOn>? AddOns { get; set; }
    Task<int> SaveChangesAsync();
}