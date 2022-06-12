using Microsoft.EntityFrameworkCore;
using Rx.Domain.Entities.Tenant;

namespace Rx.Domain.Interfaces.DbContext;

public interface ITenantDbContext:IDbContext
{
    DbSet<OrganizationCustomer>? OrganizationCustomers { get; set; }
    DbSet<Product>? Products { get; set; }
    DbSet<ProductPlan>? ProductPlans { get; set; }
    DbSet<Subscription>? Subscriptions { get; set; }
    DbSet<AddOn>? AddOns { get; set; }
    DbSet<AddOnUsage> AddOnUsages { get; set; }
    DbSet<AddOnPricePerPlan> AddOnPricePerPlans { get; set; }
    DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    DbSet<Bill> Bills { get; set; }
    DbSet<SubscriptionWebhook> SubscriptionWebhooks { get; set; }
    DbSet<AddOnWebhook> AddOnWebhooks { get; set; }
    DbSet<ChangeSubscriptionWebhook> ChangeSubscriptionWebhooks { get; set; }
    DbSet<SubscriptionStat> SubscriptionStats { get; set; }

    Task<int> SaveChangesAsync();
}