using Hangfire;
using MediatR;
using Microsoft.OpenApi.Models;
using Rx.API.Extensions;
using Rx.Application;
using Rx.Domain.Services.Primary;
using Rx.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.

builder.Services.AddPersistence(builder.Configuration);
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
builder.Services.AddCors((setup) =>
{
    setup.AddPolicy("default", (options) =>
    {
        options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("default");

app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

app.Run();