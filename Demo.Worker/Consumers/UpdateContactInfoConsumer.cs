//using System.Linq;
//using System.Threading.Tasks;
//using MassTransit;
//using Demo.Contract.Outreach;
//using Demo.Provider;

//namespace Demo.Worker.Consumers
//{
//    public class UpdateContactInfoConsumer : IConsumer<UpdateContactInformation>
//    {
//        private readonly ProviderDbContext _dbContext;

//        public UpdateContactInfoConsumer(ProviderDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task Consume(ConsumeContext<UpdateContactInformation> context)
//        {
//            var provider = _dbContext.Providers.First(p => p.Id == context.Message.ProviderId);
//            provider.EmailAddress = context.Message.EmailAddress;
//            provider.PhoneNumber = context.Message.PhoneNumber;
//            await _dbContext.SaveChangesAsync();
//        }
//    }
//}
