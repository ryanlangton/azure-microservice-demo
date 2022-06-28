using MassTransit;
using System;

namespace Demo.Worker.Consumers
{
    public class DemoConsumerDefinition : ConsumerDefinition<DemoConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator e, IConsumerConfigurator<DemoConsumer> c)
        {
            e.ConcurrentMessageLimit = 1;
            //e.ConcurrentMessageLimit = 5;

            e.UseMessageRetry(r =>
            {
                r.Immediate(5);
                r.Handle<TimeoutException>();
            });
        }
    }
}
