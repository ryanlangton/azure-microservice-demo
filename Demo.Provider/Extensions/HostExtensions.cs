using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Demo.Provider.Extensions
{
    public static class HostExtensions
    {
        public static IHost RunProviderDbMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            {
                var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
                using var dbContext = scope.ServiceProvider.GetRequiredService<ProviderDbContext>();
                dbContext.Database.Migrate();
                if (environment.IsDevelopment() && !dbContext.Providers.AnyAsync().Result)
                    AddProviderSeedData(dbContext);
            }
            return host;
        }

        private static void AddProviderSeedData(ProviderDbContext dbContext)
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
                        EmailAddress = "test@gmail.com",
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
