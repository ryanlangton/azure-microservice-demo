using MassTransit;
using QES.Messaging.ServiceBus.Configuration;
using QES.Messaging.ServiceBus.Transports;
using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QES.Messaging.ServiceBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static string ServiceBusConfigNode = "System:ServiceBus";

        public static void AddQesServiceBus(
            this IServiceCollection services,
            ServiceBusConfiguration config,
            Action<IBusRegistrationConfigurator> massTransitConfiguration = null,
            Action<IBusFactoryConfigurator> busConfiguration = null,
            Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>> customServiceInstanceConfig = null,
            bool useJobConsumers = false,
            bool scanAssemblyForConsumers = true
        )
        {
            var options = new ServiceInstanceOptions();
            if (useJobConsumers) options.EnableJobServiceEndpoints();

            services.AddMassTransit(mt =>
            {
                mt.AddDelayedMessageScheduler();

                TransportFactory.GetTransportConfigurator(config.ServiceBusUri)
                    .ConfigureServiceBus(config, mt, options, busConfiguration, customServiceInstanceConfig,
                        useJobConsumers);

                if (scanAssemblyForConsumers)
                {
                    var assembly = Assembly.GetEntryAssembly();
                    mt.AddConsumers(assembly);
                    mt.AddActivities(assembly);
                }

                massTransitConfiguration?.Invoke(mt);
            });
        }

        public static void AddQesServiceBus<T>(
            this IServiceCollection services,
            ServiceBusConfiguration config,
            Action<IBusRegistrationConfigurator<T>> massTransitConfiguration = null,
            Action<IBusFactoryConfigurator> busConfiguration = null,
            Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>> customServiceInstanceConfig = null,
            bool useJobConsumers = false,
            bool scanAssemblyForConsumers = true
        ) where T : class, IBus
        {
            var options = new ServiceInstanceOptions();
            if (useJobConsumers) options.EnableJobServiceEndpoints();

            services.AddMassTransit<T>(mt =>
            {
                mt.AddDelayedMessageScheduler();

                TransportFactory.GetTransportConfigurator(config.ServiceBusUri)
                    .ConfigureServiceBus(config, mt, options, busConfiguration, customServiceInstanceConfig,
                        useJobConsumers);

                if (scanAssemblyForConsumers)
                {
                    var assembly = Assembly.GetEntryAssembly();
                    mt.AddConsumers(assembly);
                    mt.AddActivities(assembly);
                }

                massTransitConfiguration?.Invoke(mt);
            });
        }

        public static void AddQesJobManagerBus(
            this IServiceCollection services,
            ServiceBusConfiguration config,
            Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>, IBusRegistrationContext>
                customServiceInstanceConfig,
            Action<IBusRegistrationConfigurator> massTransitConfiguration = null)
        {
            var options = new ServiceInstanceOptions();

            services.AddMassTransit(mt =>
            {
                massTransitConfiguration?.Invoke(mt);

                TransportFactory.GetTransportConfigurator(config.ServiceBusUri)
                    .ConfigureJobManagerBus(config, mt, options, customServiceInstanceConfig);
            });
        }

        public static void AddQesServiceBus(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IBusRegistrationConfigurator> massTransitConfiguration = null,
            Action<IBusFactoryConfigurator> busConfiguration = null,
            Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>> customServiceInstanceConfig = null,
            bool useJobConsumers = false,
            bool scanAssemblyForConsumers = true
        )
        {
            var config = GetConfig(configuration); 
            services.AddQesServiceBus(config, massTransitConfiguration, busConfiguration, customServiceInstanceConfig, useJobConsumers, scanAssemblyForConsumers);
        }

        public static void AddQesServiceBus<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IBusRegistrationConfigurator<T>> massTransitConfiguration = null,
            Action<IBusFactoryConfigurator> busConfiguration = null,
            Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>> customServiceInstanceConfig = null,
            bool useJobConsumers = false,
            bool scanAssemblyForConsumers = true
        ) where T : class, IBus
        {
            var config = GetConfig(configuration);
            services.AddQesServiceBus(config, massTransitConfiguration, busConfiguration, customServiceInstanceConfig, useJobConsumers, scanAssemblyForConsumers);

        }

        public static void AddQesJobManagerBus(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IServiceInstanceConfigurator<IReceiveEndpointConfigurator>, IBusRegistrationContext> customServiceInstanceConfig,
            Action<IBusRegistrationConfigurator> massTransitConfiguration = null)
        {
            var config = GetConfig(configuration);
            services.AddQesJobManagerBus(config, customServiceInstanceConfig, massTransitConfiguration);

        }

        private static ServiceBusConfiguration GetConfig(IConfiguration configuration)
        {
            var configSection = configuration.GetSection(ServiceBusConfigNode);

            if (!configSection.Exists())
                throw new Exception($"Missing configuration section for {ServiceBusConfigNode}");

            var config = configSection.Get<ServiceBusConfiguration>();

            if (string.IsNullOrEmpty(config.ServiceBusUri))
                throw new Exception($"Missing config {config}:{nameof(config.ServiceBusUri)}");

            return config;
        }
    }
}