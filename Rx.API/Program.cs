using Azure.Storage.Blobs;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;
using Rx.API.Extensions;
using Rx.Application;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Payment;
using Rx.Domain.Services.Payment;
using Rx.Domain.Services.Primary;
using Rx.Infrastructure.Persistence;
using Rx.Infrastructure.Persistence.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPrimaryDbContext, PrimaryDbContext>();
builder.Services.AddScoped<ITenantDbContext,TenantDbContext>();

builder.Services.AddPrimaryDb(builder.Configuration);
builder.Services.AddTenantDb(builder.Configuration);
builder.Services.AddSingleton<IPaymentService>(x => {
    var service = x.GetRequiredService<ILogger<PaymentService>>();
    string stripeSecretKey = builder.Configuration.GetSection("Stripe").GetValue<string>("secretKey");
    string stripePublicKey = builder.Configuration.GetSection("Stripe").GetValue<string>("publicKey");

    if (string.IsNullOrEmpty(stripeSecretKey) || string.IsNullOrEmpty(stripePublicKey))
        service.LogError("Stripe keys are missing.");
    return new PaymentService(service, stripeSecretKey);
});


builder.Services.AddBlobStorage(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureServiceManager();
//HttpClient
builder.Services.AddHttpClient();
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
    x =>
    {
        x.SwaggerDoc("v1", new OpenApiInfo { Title = "Rx API", Version = "v1" });
            x.EnableAnnotations();
    }
    );


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy");

app.UseAuthorization();

app.UseHangfireDashboard();


app.MapControllers();

app.Run();