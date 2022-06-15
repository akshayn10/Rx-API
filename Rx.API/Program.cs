using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Rx.API.Extensions;
using Rx.Application;
using Rx.Domain.Entities.Identity;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Payment;
using Rx.Domain.Interfaces.WebhookSendClient;
using Rx.Domain.Services.Payment;
using Rx.Domain.Services.WebhookSendClient;
using Rx.Infrastructure;
using Rx.Infrastructure.Identity.Seeds;
using Rx.Infrastructure.Persistence.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


//Logger configuration
var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


// Add services to the container.

//Razor Pages
builder.Services.AddControllersWithViews();

//Newtonsoft 
builder.Services.AddMvc().AddNewtonsoftJson();

//Database Settings
//DbContext
builder.Services.AddScoped<IPrimaryDbContext, PrimaryDbContext>();
builder.Services.AddScoped<ITenantDbContext,TenantDbContext>();

//Db settings from Infrastructure
builder.Services.AddPrimaryDb(builder.Configuration);
builder.Services.AddTenantDb(builder.Configuration);



//Azure Blob storage
builder.Services.AddBlobStorage(builder.Configuration);

//Add Shared infrastructure
builder.Services.AddSharedInfrastructure(builder.Configuration);

//Add IdentityInfrastructure
builder.Services.AddIdentityInfrastructure(builder.Configuration);

//Cors Settings
builder.Services.ConfigureCors();

//IIS Integration
builder.Services.ConfigureIISIntegration();

//Service Managers
builder.Services.ConfigureServices(builder.Configuration);

//HttpClient
builder.Services.AddHttpClient();
builder.Services.AddScoped<ISendWebhookService,SendWebhookService>();

//Hangfire configuration
builder.Services.AddHangfire(x =>
{
    x.UseSqlServerStorage(builder.Configuration.GetConnectionString("TenantDbConnection"));
});
builder.Services.AddHangfireServer();

//Mediatr Configuration
builder.Services.AddMediatR(typeof(ApplicationMediatrEntryPoint).Assembly);

//AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rx API", Version = "v1" });
        c.EnableAnnotations();
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Bearer",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
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
            }
        });
    }
    );


builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();


var app = builder.Build();

// Initial Users seed
 // try
 // {
 //     using (var scope = app.Services.CreateScope())
 //     {
 //         var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
 //         var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
 //         await DefaultRoles.SeedAsync(userManager,roleManager);
 //         await DefaultAdmin.SeedAsync(userManager,roleManager);
 //         await DefaultOwner.SeedAsync(userManager,roleManager);
 //         await DefaultFinanceUser.SeedAsync(userManager,roleManager);
 //     }
 // }
 // catch (Exception ex)
 // {
 //     Log.Warning(ex, "An error occurred seeding the DB");
 // }
 // finally
 // {
 //     Log.CloseAndFlush();
 // }


// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();


app.UseHangfireDashboard();

app.MapControllers();

app.Run();