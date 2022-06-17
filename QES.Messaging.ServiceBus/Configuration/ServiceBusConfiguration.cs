namespace QES.Messaging.ServiceBus.Configuration
{
    public class ServiceBusConfiguration
    {
        public ServiceBusConfiguration() { }
        public ServiceBusConfiguration(string serviceBusUri, int consumerRetries = 3)
        {
            ServiceBusUri = serviceBusUri;
            ConsumerRetries = consumerRetries;
        }
        public string ServiceBusUri { get; set; }
        public string KeyName { get; set; }
        public string SharedAccessKey { get; set; }
        public int ConsumerRetries { get; set; }
    }
}
