using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace QES.Demo.Provider.Extensions
{
    public static class ServiceScopeExtensions
    {
        public static IServiceScope AddProviderSeedData(this IServiceScope scope)
        {
            var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>().EnvironmentName;
            var dbContext = scope.ServiceProvider.GetRequiredService<ProviderDbContext>();

            if (environment.ToLower().Contains("development") && !dbContext.Providers.AnyAsync().Result)
                AddProviderSeedData(dbContext);

            return scope;
        }

        private static async Task AddProviderSeedData(ProviderDbContext dbContext)
        {
            try
            {
                var strategy = dbContext.Database.CreateExecutionStrategy();
                strategy.ExecuteInTransaction(() =>
                {
                    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Providers] ON");

                    dbContext.Providers.Add(new Model.Provider
                    {
                        Id = 1,
                        EmailAddress = "test@questanalytics.com",
                        PhoneNumber = "704-560-2464"
                    });

                    dbContext.SaveChanges();
                    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Providers] OFF");
                }, () => true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
