using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Demo.Data
{
    // <summary>
    // This is used to create migrations locally
    // </summary>
    public class DemoDbContextFactory : IDesignTimeDbContextFactory<DemoDbContext>
    {
        public DemoDbContext CreateDbContext(string[] args)
        {
            var directory = $"{Directory.GetCurrentDirectory()}/../QESI.Demo.Worker";
            var builder = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.development.json");

            var config = builder.Build();

            var connstr = config.GetConnectionString("DemoDb");

            var optionsBuilder = new DbContextOptionsBuilder<DemoDbContext>();
            optionsBuilder.UseSqlServer(connstr);

            return new DemoDbContext(optionsBuilder.Options);
        }
    }
}
