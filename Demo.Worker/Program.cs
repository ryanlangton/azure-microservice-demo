using System;
using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Demo.Domain.Configuration;
using Demo.Domain.Extensions;
using Demo.Provider.Extensions;
using Demo.Saga.Extensions;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureAppConfiguration((hostContext, config) =>
    {
        var settings = config.Build();
        var connection = settings.GetConnectionString("AppConfig");
        config.AddAzureAppConfiguration(opt => opt.Connect(connection).UseFeatureFlags(), optional: true)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
    })
    .UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console())
    .ConfigureServices((hostContext, services) =>
    {
        // Adding custom app config
        services.Configure<AppConfiguration>(hostContext.Configuration.GetSection("AppSettings"));
        var healthChecks = services.AddHealthChecks();

        // Add demo services
        services.AddDemoDomain();
        var connectionString = hostContext.Configuration.GetConnectionString("DemoDb");
        services.AddProviderData(healthChecks, connectionString);
        services.AddSagaData(healthChecks, connectionString);

        // Add MT service bus 
        services.AddMassTransit(mt =>
        {
            mt.AddDelayedMessageScheduler();

            mt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(hostContext.Configuration.GetValue<string>("ServiceBus:Uri"), host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });
                cfg.UseDelayedMessageScheduler();
                cfg.UseInMemoryOutbox();
                cfg.ConfigureEndpoints(context);
            });

            mt.AddConsumers(Assembly.GetEntryAssembly());
        });

        // Add external services
        services.AddHttpClient();
        services.AddFeatureManagement();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    });

try
{
    builder.Build()
        .RunProviderDbMigrations()
        .RunSagaDbMigrations()
        .ValidateAutomapper()
        .Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.ReadKey();
    throw;
}
