using MassTransit;
using QES.Messaging.ServiceBus.Configuration;
using System;

namespace QES.Messaging.ServiceBus.Transports
{
    public class InMemoryConfigurator : IQesiBusConfigurator
    {
        private readonly Uri schedulerEndpoint = new Uri("queue:scheduler");

        public void ConfigureServiceBus(ServiceBusConfiguration config, IBusRegistrationConfigurator mt, ServiceInstanceOptions options, Action<IBusFactoryConfigurator> busConfiguration = null, Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>> customServiceInstanceConfig = null, bool useJobServices = false)
        {
            mt.UsingInMemory((context, cfg) =>
            {
                cfg.UseMessageScheduler(schedulerEndpoint);
                busConfiguration?.Invoke(cfg);

                if (!useJobServices)
                    cfg.ConfigureEndpoints(context);
                else
                {
                    cfg.ServiceInstance(options, instance =>
                    {
                        customServiceInstanceConfig?.Invoke(instance);
                        instance.ConfigureEndpoints(context);
                    });
                }
            });
        }

        public void ConfigureJobManagerBus(ServiceBusConfiguration config, IBusRegistrationConfigurator mt, ServiceInstanceOptions options, Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>, IBusRegistrationContext> customServiceInstanceConfig)
        {
            mt.UsingInMemory((context, cfg) =>
            {
                cfg.UseMessageScheduler(schedulerEndpoint);
                cfg.ServiceInstance(options, instance =>
                {
                    customServiceInstanceConfig?.Invoke(instance, context);
                });
            });
        }
    }
}

