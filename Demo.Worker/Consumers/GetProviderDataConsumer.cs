using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Demo.Contract.Outreach.Consumer;
using Demo.Provider;

namespace Demo.Worker.Consumers
{
    public class GetProviderDataConsumer : IConsumer<GetProviderData>
    {
        private readonly ProviderDbContext _dbContext;

        public GetProviderDataConsumer(ProviderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<GetProviderData> context)
        {
            var provider = await _dbContext.Providers.FirstAsync(p => p.Id == context.Message.ProviderId);
            var providerData = new
            { context.Message.CorrelationId, provider.EmailAddress, provider.PhoneNumber };
            await context.Publish<GetProviderDataSuccess>(providerData);
        }
    }
}
