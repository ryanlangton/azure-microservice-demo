using MassTransit;
using StatsdClient;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Util;

namespace QES.Messaging.ServiceBus.Middleware
{
    public class DataDogStateChangeObserver<T> :
            IStateObserver<T>
            where T : class, ISaga
    {
        private readonly DogStatsdService dogStatsd;
        private readonly State[] errorStates;

        public DataDogStateChangeObserver(DogStatsdService dogStatsd, params State[] errorStates)
        {
            this.dogStatsd = dogStatsd;
            this.errorStates = errorStates;
        }

        public Task StateChanged(BehaviorContext<T> context, State currentState, State previousState)
        {
            if (errorStates.Contains(currentState))
                dogStatsd.Event($"{typeof(T).Name} Change", $"Saga {context.Saga.CorrelationId} entered State: {currentState.Name}. Left state: {previousState?.Name}.", alertType: "error");

            dogStatsd.Event($"{typeof(T).Name} Change", $"Saga {context.Saga.CorrelationId} entered State: {currentState.Name}. Left state: {previousState?.Name}.");
            return TaskUtil.Completed;
        }
    }
}
