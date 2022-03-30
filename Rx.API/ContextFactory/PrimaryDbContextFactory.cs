

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Rx.Infrastructure.Persistence.Context;

namespace Rx.API.ContextFactory
{
    //public class PrimaryDbContextFactory:IDesignTimeDbContextFactory<PrimaryDbContext>
    //{
    //    public PrimaryDbContext CreateDbContext(string[] args)
    //    {
    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json")
    //            .Build();

    //        var builder = new DbContextOptionsBuilder<PrimaryDbContext>()
    //            .UseSqlServer(configuration.GetConnectionString("PrimaryDbConnection"),
    //                b => b.MigrationsAssembly("RxAPI"));

    //        return new PrimaryDbContext(builder.Options);

    //    }
    //}
}
