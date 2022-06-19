using Microsoft.Extensions.DependencyInjection;

namespace Demo.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDemoDomain(this IServiceCollection services)
        {
            services.Scan(scan =>
                scan.FromExecutingAssembly()
                    .AddClasses()
                    .AsMatchingInterface());
        }
    }
}
