using MassTransit;
using System;

namespace QES.Messaging.ServiceBus.Extensions
{
    public static class ConsumePipeConfiguratorExtensions
    {
        /// <summary>
        /// A standardized retry which triggers at exponentially larger intervals.
        /// </summary>
        /// <param name="configurator"></param>
        public static void UseIntervals(this IConsumePipeConfigurator configurator, int numRetries)
        {
            if (numRetries <= 0) return;                
            configurator.UseMessageRetry(retry => retry.Exponential(numRetries, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(10), TimeSpan.FromSeconds(10)));
        }
    }
}
