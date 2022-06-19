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

var builder = Host.CreateDefaultBuilder();

builder.ConfigureAppConfiguration((hostingContext, config) =>
    {
        var settings = config.Build();
        var connection = settings.GetConnectionString("AppConfig");
        config.AddAzureAppConfiguration(opt => opt.Connect(connection).UseFeatureFlags(), optional: true)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
    })
    //.ConfigureQesLogging("Demo Worker")
    .ConfigureServices((hostContext, services) =>
    {
        // Adding custom app config
        services.Configure<AppConfiguration>(hostContext.Configuration.GetSection("AppSettings"));
        var healthChecks = services.AddHealthChecks();

        // Add QES demo services
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
