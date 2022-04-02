using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rx.Domain.Interfaces.DbContext;
using Rx.Infrastructure.Persistence.Context;

namespace Rx.Infrastructure.Persistence
{
    public static class Dependencies
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PrimaryDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("PrimaryDbConnection"),
                    b => b.MigrationsAssembly(typeof(PrimaryDbContext).Assembly.FullName)));

            services.AddScoped<IPrimaryDbContext>(provider => provider.GetService<PrimaryDbContext>() ?? throw new InvalidOperationException());
            

            services.AddDbContext<TenantDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("TenantDbConnection"),
                    b => b.MigrationsAssembly(typeof(TenantDbContext).Assembly.FullName)));

            services.AddScoped<ITenantDbContext>(provider => provider.GetService<TenantDbContext>() ?? throw new InvalidOperationException());
        }

    }
}
