using System;
using System.Reflection;
using Demo.Domain.Configuration;
using Demo.Domain.Extensions;
using Demo.Publisher;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = Host.CreateDefaultBuilder();

builder.ConfigureAppConfiguration((hostingContext, config) =>
    {
        var settings = config.Build();
        var connection = settings.GetConnectionString("AppConfig");
        config
            //.AddAzureAppConfiguration(opt => opt.Connect(connection).UseFeatureFlags(), optional: true)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
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
        
        services.AddHealthChecks();
        services.AddDemoDomain();

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
        });

        // Add external services
        services.AddHttpClient();
        services.AddFeatureManagement();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        //services.AddSingleton(Log.Logger);
        services.AddHostedService<MessagePublisher>();
    });

try
{
    builder.Build()
        .ValidateAutomapper()
        .Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.ReadKey();
    throw;
}
