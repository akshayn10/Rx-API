using MediatR;
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
builder.Services.AddHttpClient<OrganizationService>();
builder.Services.AddMediatR(typeof(ApplicationMediatrEntryPoint).Assembly);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();