using MassTransit;
using QES.Messaging.ServiceBus.Configuration;
using System;

namespace QES.Messaging.ServiceBus.Transports
{
    public interface IQesiBusConfigurator
    {
        void ConfigureServiceBus(ServiceBusConfiguration config, IBusRegistrationConfigurator mt, ServiceInstanceOptions options, Action<IBusFactoryConfigurator> busConfiguration = null, Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>> customServiceInstanceConfig = null, bool useJobConsumers = false);
        void ConfigureJobManagerBus(ServiceBusConfiguration config, IBusRegistrationConfigurator mt, ServiceInstanceOptions options, Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>, IBusRegistrationContext> customServiceInstanceConfig);
    }
}
