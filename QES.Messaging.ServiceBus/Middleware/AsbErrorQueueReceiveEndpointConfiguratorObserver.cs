using MassTransit;

namespace QES.Messaging.ServiceBus.Middleware
{
    public class AsbErrorQueueReceiveEndpointConfiguratorObserver : IEndpointConfigurationObserver
    {
        public void EndpointConfigured<T>(T configurator)
            where T : IReceiveEndpointConfigurator
        {
            configurator.ConfigureDeadLetterQueueDeadLetterTransport();
            configurator.ConfigureDeadLetterQueueErrorTransport();
        }
    }
}
