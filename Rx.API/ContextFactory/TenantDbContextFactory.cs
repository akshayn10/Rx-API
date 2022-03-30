using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Rx.Infrastructure.Persistence.Context;

namespace Rx.API.ContextFactory
{
    //public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
    //{
    //    public TenantDbContext CreateDbContext(string[] args)
    //        {
    //            var configuration = new ConfigurationBuilder()
    //                .SetBasePath(Directory.GetCurrentDirectory())
    //                .AddJsonFile("appsettings.json")
    //                .Build();

    //            var builder = new DbContextOptionsBuilder<TenantDbContext>()
    //                .UseSqlServer(configuration.GetConnectionString("TenantDbConnection"),
    //                    b => b.MigrationsAssembly("RxAPI"));

    //            return new TenantDbContext(builder.Options);
    //        }
    //    }
    }