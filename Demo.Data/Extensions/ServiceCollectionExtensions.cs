using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Demo.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDemoData(this IServiceCollection services, IHealthChecksBuilder healthChecks, string connectionString)
        {
            services.AddDbContext<DemoDbContext>(opt => opt.UseSqlServer(connectionString));

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
