using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using QES.Demo.Domain.Configuration;
using QES.Demo.Domain.Extensions;
using QES.Demo.Publisher;
using QES.Logging.Extensions;
using QES.Messaging.ServiceBus.Extensions;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureAppConfiguration((hostingContext, config) =>
    {
        var settings = config.Build();
        var connection = settings.GetConnectionString("AppConfig");
        config.AddAzureAppConfiguration(opt => opt.Connect(connection).UseFeatureFlags(), optional: true)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
    })
    .ConfigureQesLogging("Demo Worker")
    .ConfigureServices((hostContext, services) =>
    {
        // Adding custom app config
        services.Configure<DemoConfiguration>(hostContext.Configuration.GetSection(DemoConstants.ConfigKey));

        // Add QES demo services
        services.AddDemoDomain();

        // Add MT service bus
        services.AddQesServiceBus(hostContext.Configuration);

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
