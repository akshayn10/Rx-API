using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Rx.Domain.Entities.Identity;
using Rx.Domain.Interfaces.UtcDateTime;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.Blob;
using Rx.Domain.Interfaces.Email;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Settings;
using Rx.Domain.Wrappers;
using Rx.Infrastructure.Identity.Contexts;
using Rx.Infrastructure.Identity.Service;
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
            services.AddTransient<IDateTimeService, DateTimeService>();
        }
         public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
   
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(
                    configuration.GetConnectionString("PrimaryDbConnection"),
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            
            services.AddIdentity<ApplicationUser, IdentityRole>(o=>
            {
                o.User.RequireUniqueEmail = true;
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 8;

            }
                ).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            #region Services
            services.AddTransient<IUserService, UserService>();
            #endregion

            // services.AddScoped<IIdentityContext,IdentityContext>();
            services.Configure<JwtSecurityTokenSettings>(configuration.GetSection("JwtSecurityTokenSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSecurityTokenSettings:Issuer"],
                        ValidAudience = configuration["JwtSecurityTokenSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityTokenSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new ResponseMessage<string>("You are not Authorized"));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new ResponseMessage<string>("You are not authorized to access this resource"));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }
        
    }
}