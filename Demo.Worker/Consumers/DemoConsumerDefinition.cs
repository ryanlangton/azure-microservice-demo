using MassTransit;

namespace Demo.Worker.Consumers
{
    public class DemoConsumerDefinition : ConsumerDefinition<DemoConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<DemoConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConcurrentMessageLimit = 1;
        }
    }
}
