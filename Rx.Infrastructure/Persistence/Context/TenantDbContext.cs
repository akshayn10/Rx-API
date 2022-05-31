using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces.DbContext;
namespace Rx.Infrastructure.Persistence.Context
{
    public class TenantDbContext :DbContext,ITenantDbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<OrganizationCustomer>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
        public DbSet<OrganizationCustomer>? OrganizationCustomers { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<ProductPlan>? ProductPlans { get; set; }
        public DbSet<Subscription>? Subscriptions { get; set; }
        public DbSet<Bill>? Bills { get; set; }
        public DbSet<PaymentTransaction>? PaymentTransactions { get; set; }
        public DbSet<AddOn>? AddOns { get; set; }
        public DbSet<AddOnPricePerPlan>? AddOnPricePerPlans { get; set; }
        public DbSet<AddOnUsage>? AddOnUsages { get; set; }
        
        //Webhooks Storage
        public DbSet<SubscriptionWebhook>? SubscriptionWebhooks { get; set; }
        public DbSet<AddOnWebhook>? AddOnWebhooks { get; set; }
        
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
