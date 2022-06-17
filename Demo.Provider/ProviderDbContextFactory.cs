using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace QES.Demo.Provider
{
    // <summary>
    // This is used to create migrations locally
    // </summary>
    public class ProviderDbContextFactory : IDesignTimeDbContextFactory<ProviderDbContext>
    {
        public ProviderDbContext CreateDbContext(string[] args)
        {
            var directory = $"{Directory.GetCurrentDirectory()}/../QES.Demo.Worker";
            var builder = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.development.json");

            var config = builder.Build();

            var connstr = config.GetConnectionString("DemoDb");

            var optionsBuilder = new DbContextOptionsBuilder<ProviderDbContext>();
            optionsBuilder.UseSqlServer(connstr);

            return new ProviderDbContext(optionsBuilder.Options);
        }
    }
}
