using System.Threading.Tasks;
using MassTransit;
using Demo.Contract;
using ILogger = Serilog.ILogger;
using System;

namespace Demo.Worker.Consumers
{
    public class DemoConsumer : IConsumer<DemoMessage>
    {
        private readonly ILogger _logger;

        public DemoConsumer(ILogger logger)
        {
            _logger = logger.ForContext<DemoConsumer>();
        }

        public async Task Consume(ConsumeContext<DemoMessage> context)
        {
            //throw new Exception("Something went wrong!");
            //throw new TimeoutException("Timeout occurred!");
            _logger.Information("Message received #{MessageNumber}, CorrelationID {CorrelationId}!", context.Message.MessageNumber, context.CorrelationId);
            await Task.Delay(2000);
            _logger.Information("Message processed #{MessageNumber}, CorrelationID {CorrelationId}!", context.Message.MessageNumber, context.CorrelationId);
        }
    }
}
