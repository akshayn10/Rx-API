using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
                        .WithOrigins("http://localhost:4200", "https://victorious-ocean-080d44f00.1.azurestaticapps.net")
                );
            });

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options => { });

        public static void ConfigureServices(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddScoped<IPrimaryServiceManager, PrimaryServiceManager>();
            services.AddScoped<ITenantServiceManager, TenantServiceManager>();
            services.AddScoped<IBillingService, BillingService>();

            //Stripe Settings
            services.AddSingleton<IPaymentService>(x =>
            {
                var service = x.GetRequiredService<ILogger<PaymentService>>();
                string stripeSecretKey = configuration.GetSection("Stripe").GetValue<string>("secretKey");
                string stripePublicKey = configuration.GetSection("Stripe").GetValue<string>("publicKey");

                if (string.IsNullOrEmpty(stripeSecretKey) || string.IsNullOrEmpty(stripePublicKey))
                    service.LogError("Stripe keys are missing.");
                return new PaymentService(service, stripeSecretKey);
            });
        }

        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Project Rx- WebApi",
                    Description = "This Api will be responsible for overall data distribution and authorization.",
                    Contact = new OpenApiContact
                    {
                        Name = "Team Backslash",
                        Email = "fit.team.backslash@gmail.com"
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description =
                        "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                });
            });

        }
    }
}
