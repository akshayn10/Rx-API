using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rx.Domain.Interfaces.Blob;
using Rx.Domain.Interfaces.Email;
using Rx.Domain.Settings;
using Rx.Infrastructure.Persistence;
using Rx.Infrastructure.Persistence.Context;
using Rx.Infrastructure.Shared;

namespace Rx.Infrastructure
{
    public static class Dependencies
    {
        public static void AddPrimaryDb(this IServiceCollection services, IConfiguration configuration)
        {
            // services.AddScoped<IPrimaryDbContext, PrimaryDbContext>();
            services.AddDbContext<PrimaryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PrimaryDbConnection")
                    // , b => b.MigrationsAssembly(typeof(PrimaryDbContext).Assembly.FullName)
                    )
                //, ServiceLifetime.Transient
            );
            // services.AddTransient<IPrimaryDbContext>(provider => provider.GetService<PrimaryDbContext>() ?? throw new InvalidOperationException());
        }
        public static void AddTenantDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TenantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TenantDbConnection")
                    // , b => b.MigrationsAssembly(typeof(TenantDbContext).Assembly.FullName)
                    )
                // , ServiceLifetime.Transient
                );
        }
        public static void AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(b=>
            {
                b.AddBlobServiceClient(configuration.GetConnectionString("productBlobStorageConnectionString"));
            });
            services.AddScoped<IBlobStorage, BlobStorage>();
        }

        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}