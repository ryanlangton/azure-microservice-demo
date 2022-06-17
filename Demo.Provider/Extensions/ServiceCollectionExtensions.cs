using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using QES.EntityFramework.Extensions;

namespace QES.Demo.Provider.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderData(this IServiceCollection services, IHostEnvironment environment, IHealthChecksBuilder healthChecks, string connectionString)
        {
            services.AddDbContextResiliency<ProviderDbContext>(connectionString, environment);

            healthChecks.AddSqlServer(
                    connectionString: connectionString,
                    healthQuery: "SELECT 1;",
                    name: "provider sql",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] {"db", "sql", "sqlserver"});

            services.Scan(scan =>
                scan.FromAssemblyOf<ProviderDbContext>()
                    .AddClasses(false)
                    .AsMatchingInterface()
                    .WithScopedLifetime());

            return services;
        }
    }
}
