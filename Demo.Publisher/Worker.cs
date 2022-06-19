using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Demo.Contract;
using Demo.Contract.Outreach;
using ILogger = Serilog.ILogger;

namespace Demo.Publisher
{
    public class MessagePublisher : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IBusControl _bus;
        private readonly int ProviderId = 1;

        public MessagePublisher(ILogger logger, IBusControl bus)
        {
            _logger = logger.ForContext<MessagePublisher>();
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.WaitForHealthStatus(BusHealthStatus.Healthy, stoppingToken);
            try
            {
                var correlationId = Guid.Empty;
                while (true)
                {
                    Console.WriteLine("What would you like to do?");
                    Console.WriteLine("A) Begin a new outreach");
                    Console.WriteLine("B) Request email outreach");
                    Console.WriteLine("C) Report email outreach success");
                    Console.WriteLine("D) Request phone outreach");
                    Console.WriteLine("E) Report phone outreach success");
                    Console.WriteLine("F) Update contact information");
                    Console.WriteLine("Try scaling (type any number from 1-200+)");
                    Console.WriteLine("Q to Quit");

                    var userEntry = Console.ReadLine();

                    if (int.TryParse(userEntry, out var messageCount))
                    {
                        for (var i = 0; i < messageCount; i++)
                        {
                            // IBus is a last resort publisher, prefer the IPublishEndpoint if you're in a scoped service
                            // https://masstransit-project.com/usage/producers.html#publish
                            await _bus.Publish<DemoMessage>(new { CorrelationId = NewId.NextGuid(), MessageNumber = i }, stoppingToken);
                        }
                        continue;
                    }

                    if (userEntry == "Q")
                        break;

                    correlationId = await PublishSagaMessage(userEntry, stoppingToken, correlationId);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error starting demo worker");
                throw;
            }
        }

        private async Task<Guid> PublishSagaMessage(string userEntry, CancellationToken stoppingToken, Guid correlationId)
        {
            switch (userEntry)
            {
                case "A":
                    correlationId = Guid.NewGuid();
                    await _bus.Publish<BeginOutreach>(new { correlationId, ProviderId }, stoppingToken);
                    break;
                case "B":
                    await _bus.Publish<StartOutreachEmail>(new { correlationId }, stoppingToken);
                    break;
                case "C":
                    await _bus.Publish<OutreachEmailSuccess>(new { correlationId }, stoppingToken);
                    break;
                case "D":
                    await _bus.Publish<StartOutreachPhoneCall>(new { correlationId }, stoppingToken);
                    break;
                case "E":
                    await _bus.Publish<OutreachPhoneCallSuccess>(new { correlationId }, stoppingToken);
                    break;
                case "F":
                    Console.WriteLine("Enter email address:");
                    var email = Console.ReadLine();
                    Console.WriteLine("Enter phone number:");
                    var phoneNumber = Console.ReadLine();
                    await _bus.Publish<UpdateContactInformation>(new { correlationId, ProviderId, phoneNumber, email }, stoppingToken);
                    break;
                default:
                    Console.WriteLine($"Invalid input {userEntry}, try again");
                    break;
            }

            return correlationId;
        }
    }
}
