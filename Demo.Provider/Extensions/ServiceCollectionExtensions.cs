using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Demo.Provider.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderData(this IServiceCollection services, IHealthChecksBuilder healthChecks, string connectionString)
        {
            services.AddDbContext<ProviderDbContext>(opt => opt.UseSqlServer(connectionString));

            healthChecks.AddSqlServer(
                    connectionString: connectionString,
                    healthQuery: "SELECT 1;",
                    name: "provider sql",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "db", "sql", "sqlserver" });

            services.Scan(scan =>
                scan.FromAssemblyOf<ProviderDbContext>()
                    .AddClasses()
                    .AsMatchingInterface()
                    .WithScopedLifetime());

            return services;
        }
    }
}
