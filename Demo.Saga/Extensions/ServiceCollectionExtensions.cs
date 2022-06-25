using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Demo.Saga.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSagaData(this IServiceCollection services, IHealthChecksBuilder healthChecks, string connectionString)
        {
            services.AddDbContext<OutreachStateDbContext>(opt => opt.UseSqlServer(connectionString));

            healthChecks.AddSqlServer(
                    connectionString: connectionString,
                    healthQuery: "SELECT 1;",
                    name: "outreach state sql",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "db", "sql", "sqlserver" });

            services.Scan(scan =>
                scan.FromAssemblyOf<OutreachStateDbContext>()
                    .AddClasses()
                    .AsMatchingInterface()
                    .WithScopedLifetime());

            return services;
        }
    }
}
