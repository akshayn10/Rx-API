using Microsoft.EntityFrameworkCore;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.Payment;
using Rx.Domain.Interfaces.Tenant;
using Rx.Domain.Services.Payment;
using Rx.Domain.Services.Primary;
using Rx.Domain.Services.Tenant;


namespace Rx.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("http://localhost:4200")
                    );
            });
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
            });

        public static void ConfigureServices(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddScoped<IPrimaryServiceManager, PrimaryServiceManager>();
            services.AddScoped<ITenantServiceManager, TenantServiceManager>();
            services.AddScoped<IBillingService, BillingService>();
            
            //Stripe Settings
            services.AddSingleton<IPaymentService>(x => {
                var service = x.GetRequiredService<ILogger<PaymentService>>();
                string stripeSecretKey = configuration.GetSection("Stripe").GetValue<string>("secretKey");
                string stripePublicKey = configuration.GetSection("Stripe").GetValue<string>("publicKey");

                if (string.IsNullOrEmpty(stripeSecretKey) || string.IsNullOrEmpty(stripePublicKey))
                    service.LogError("Stripe keys are missing.");
                return new PaymentService(service, stripeSecretKey);
            });
        }
        
    }
}
