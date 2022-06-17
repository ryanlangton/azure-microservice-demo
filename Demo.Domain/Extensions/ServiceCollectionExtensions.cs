using Microsoft.Extensions.DependencyInjection;

namespace QES.Demo.Domain.Extensions
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
