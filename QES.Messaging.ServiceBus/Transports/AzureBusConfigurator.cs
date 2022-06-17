using MassTransit;
using QES.Messaging.ServiceBus.Configuration;
using QES.Messaging.ServiceBus.Middleware;
using System;

namespace QES.Messaging.ServiceBus.Transports
{
    public class AzureBusConfigurator : IQesiBusConfigurator
    {
        public void ConfigureServiceBus(ServiceBusConfiguration config, IBusRegistrationConfigurator mt, ServiceInstanceOptions options, Action<IBusFactoryConfigurator> busConfiguration = null, Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>> customServiceInstanceConfig = null, bool useJobConsumers = false)
        {
            mt.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.ConnectEndpointConfigurationObserver(new AsbErrorQueueReceiveEndpointConfiguratorObserver());
                var serviceBusUri = config.ServiceBusUri.StartsWith("Endpoint=") ?
                    config.ServiceBusUri :
                    $"Endpoint={config.ServiceBusUri};SharedAccessKeyName={config.KeyName};SharedAccessKey={config.SharedAccessKey}";
                cfg.Host(serviceBusUri);

                cfg.UseServiceBusMessageScheduler();
                cfg.UseInMemoryOutbox();

                busConfiguration?.Invoke(cfg);

                if (!useJobConsumers)
                    cfg.ConfigureEndpoints(context);
                else
                {
                    cfg.ServiceInstance(options, instance =>
                    {
                        customServiceInstanceConfig?.Invoke(instance);
                        instance.ConfigureJobService();
                        instance.ConfigureEndpoints(context);
                    });
                }
                cfg.LockDuration = TimeSpan.FromMinutes(5);
                cfg.MaxAutoRenewDuration = TimeSpan.FromHours(1);
            });
        }

        public void ConfigureJobManagerBus(ServiceBusConfiguration config, IBusRegistrationConfigurator mt, ServiceInstanceOptions options, Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>, IBusRegistrationContext> customServiceInstanceConfig)
        {
            mt.UsingAzureServiceBus((context, cfg) =>
            {
                var serviceBusUri = config.ServiceBusUri.StartsWith("Endpoint=") ?
                    config.ServiceBusUri :
                    $"Endpoint={config.ServiceBusUri};SharedAccessKeyName={config.KeyName};SharedAccessKey={config.SharedAccessKey}";
                cfg.Host(serviceBusUri);
                cfg.UseServiceBusMessageScheduler();
                cfg.UseInMemoryOutbox();

                cfg.ServiceInstance(options, instance =>
                {
                    customServiceInstanceConfig?.Invoke(instance, context);
                });
            });
        }
    }
}

