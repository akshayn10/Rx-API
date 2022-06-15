using Microsoft.EntityFrameworkCore;
using Rx.Domain.Entities.Primary;

namespace Rx.Domain.Interfaces.DbContext
{
    public interface IPrimaryDbContext:IDbContext
   
    {
            DbSet<Organization>? Organizations { get; set; }
            DbSet<OrganizationAddress>? OrganizationAddresses { get; set; }
            DbSet<SystemSubscription>? SystemSubscriptions { get; set; }
            DbSet<SystemSubscriptionPlan> SystemSubscriptionPlans { get; set; }
            DbSet<PaymentTransaction> PaymentTransactions { get; set; }
            DbSet<Bill> Bills { get; set; }
            Task<int> SaveChangesAsync();
    }
}