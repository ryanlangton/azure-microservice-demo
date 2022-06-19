using System;
using Demo.Saga.Model;
using MassTransit;

namespace Demo.Saga
{
    public class OutreachStateMachineDefinition : SagaDefinition<OutreachState>
    {
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OutreachState> sagaConfigurator)
        {
            endpointConfigurator.UseRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
            base.ConfigureSaga(endpointConfigurator, sagaConfigurator);
        }
    }
}
