using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.DbContext;
using Rx.Infrastructure.Persistence.Configuration.Primary;

namespace Rx.Infrastructure.Persistence.Context
{
    public class PrimaryDbContext : DbContext,IPrimaryDbContext
    {
        public PrimaryDbContext(DbContextOptions<PrimaryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new OrganizationConfiguration());
        }
        
        public DbSet<Organization>? Organizations { get; set; }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
