using Microsoft.EntityFrameworkCore;
using Rx.Domain.Interfaces;
using Rx.Domain.Services.Primary;
using Rx.Domain.Services.Tenant;


namespace Rx.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
            });

        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IPrimaryServiceManager, PrimaryServiceManager>();
            services.AddScoped<ITenantServiceManager, TenantServiceManager>();


        }


    }
}
