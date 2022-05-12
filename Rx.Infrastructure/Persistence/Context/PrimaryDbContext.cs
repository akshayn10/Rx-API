using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.DbContext;

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
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        
        public DbSet<Organization>? Organizations { get; set; }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
