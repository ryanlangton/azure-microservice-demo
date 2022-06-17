using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace QES.Demo.Domain.Extensions
{
    public static class HostExtensions
    {
        public static IHost ValidateAutomapper(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            return host;
        }
    }
}
