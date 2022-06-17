using MassTransit;
using QES.Messaging.ServiceBus.Configuration;
using System;

namespace QES.Messaging.ServiceBus.Transports
{
    public class RabbitMqBusConfigurator : IQesiBusConfigurator
    {
        public void ConfigureServiceBus(ServiceBusConfiguration config, IBusRegistrationConfigurator mt, ServiceInstanceOptions options, Action<IBusFactoryConfigurator> busConfiguration = null, Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>> customServiceInstanceConfig = null, bool useJobConsumers = false)
        {
            mt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config.ServiceBusUri, host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });
                cfg.UseDelayedMessageScheduler();
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
            });
        }

        public void ConfigureJobManagerBus(ServiceBusConfiguration config, IBusRegistrationConfigurator mt, ServiceInstanceOptions options, Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>, IBusRegistrationContext> customServiceInstanceConfig)
        {
            mt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config.ServiceBusUri, host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });
                cfg.UseDelayedMessageScheduler();
                cfg.UseInMemoryOutbox();

                cfg.ServiceInstance(options, instance =>
                {
                    customServiceInstanceConfig?.Invoke(instance, context);
                });

            });
        }
    }
}
