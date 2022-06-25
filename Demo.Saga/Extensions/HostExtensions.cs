using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.Saga.Extensions
{
    public static class HostExtensions
    {
        public static IHost RunSagaDbMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<OutreachStateDbContext>();
                dbContext.Database.Migrate();
            }
            return host;
        }
    }

}
