using System;
using Demo.Publisher;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        config
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
            });
        });
        services.AddHostedService<MessagePublisher>();
    });

try
{
    builder.Build().Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.ReadKey();
    throw;
}
