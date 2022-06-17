using QES.Messaging.ServiceBus.Configuration;

namespace QES.Messaging.ServiceBus.Transports
{
    public static class TransportFactory
    {
        public static IQesiBusConfigurator GetTransportConfigurator(string serviceBusUri)
        {
            if (serviceBusUri.StartsWith("rabbitmq"))
                return new RabbitMqBusConfigurator();
            if (serviceBusUri.StartsWith("inmemory"))
                return new InMemoryConfigurator();
            return new AzureBusConfigurator();
        }
    }
}
