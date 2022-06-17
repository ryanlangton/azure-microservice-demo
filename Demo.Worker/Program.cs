using System;
using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using QES.Demo.Domain.Configuration;
using QES.Demo.Domain.Extensions;
using QES.Demo.Provider;
using QES.Demo.Provider.Extensions;
using QES.Demo.Saga;
using QES.Demo.Saga.Extensions;
using QES.Demo.Saga.Model;
using QES.EntityFramework.Extensions;
using QES.Logging.Extensions;
using QES.Messaging.ServiceBus.Extensions;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureAppConfiguration((hostContext, config) =>
    {
        var settings = config.Build();
        var connection = settings.GetConnectionString("AppConfig");
        config.AddAzureAppConfiguration(opt => opt.Connect(connection).UseFeatureFlags(), optional: true)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
    })
    .ConfigureQesLogging("Demo Worker")
    .ConfigureServices((hostContext, services) =>
    {
        // Adding custom app config
        services.Configure<DemoConfiguration>(hostContext.Configuration.GetSection(DemoConstants.ConfigKey));
        var healthChecks = services.AddHealthChecks();

        // Add QES demo services
        services.AddDemoDomain();
        var connectionString = hostContext.Configuration.GetConnectionString("DemoDb");
        services.AddProviderData(hostContext.HostingEnvironment, healthChecks, connectionString);
        services.AddSagaData(hostContext.HostingEnvironment, healthChecks, connectionString);

        // Add MT service bus
        services.AddQesServiceBus(hostContext.Configuration, config =>
        {
            config.AddSagaStateMachine<OutreachStateMachine, OutreachState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                    r.ExistingDbContext<OutreachStateDbContext>();
                    r.CustomizeQuery(q =>
                    {
                        return q
                            .Include(o => o.OutreachEmailAttempts)
                            .Include(o => o.OutreachPhoneAttempts);
                    });
                });
        });

        // Add external services
        services.AddHttpClient();
        services.AddFeatureManagement();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    });

try
{
    builder.Build()
        .ApplyDbContextMigrations<ProviderDbContext>()
        .AddProviderSeedData()
        .ApplyDbContextMigrations<OutreachStateDbContext>()
        .ValidateAutomapper()
        .Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.ReadKey();
    throw;
}
