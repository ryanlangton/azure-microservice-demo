using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using QES.EntityFramework.Extensions;

namespace QES.Demo.Saga.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSagaData(this IServiceCollection services, IHostEnvironment environment, IHealthChecksBuilder healthChecks, string connectionString)
        {
            services.AddDbContextResiliency<OutreachStateDbContext>(connectionString, environment);

            healthChecks.AddSqlServer(
                    connectionString: connectionString,
                    healthQuery: "SELECT 1;",
                    name: "outreach state sql",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] {"db", "sql", "sqlserver"});

            services.Scan(scan =>
                scan.FromAssemblyOf<OutreachStateDbContext>()
                    .AddClasses(false)
                    .AsMatchingInterface()
                    .WithScopedLifetime());

            return services;
        }
    }
}
