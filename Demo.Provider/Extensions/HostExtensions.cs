using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace QES.Demo.Provider.Extensions
{
    public static class HostExtensions
    {
        public static IHost AddProviderSeedData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            scope.AddProviderSeedData();
            return host;
        }
    }
}
