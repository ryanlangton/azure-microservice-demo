using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using QES.EntityFramework.Extensions;

namespace QES.Demo.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDemoData(this IServiceCollection services, IHostEnvironment environment, IHealthChecksBuilder healthChecks, string connectionString)
        {
            services.AddDbContextResiliency<DemoDbContext>(connectionString, environment);

            healthChecks.AddSqlServer(
                connectionString,
                healthQuery: "SELECT 1;",
                name: "sql",
                failureStatus: HealthStatus.Degraded,
                tags: new string[] { "db", "sql", "sqlserver" });

            services.Scan(scan =>
                scan.FromExecutingAssembly()
                    .AddClasses()
                    .AsMatchingInterface());

            return services;
        }
    }
}
