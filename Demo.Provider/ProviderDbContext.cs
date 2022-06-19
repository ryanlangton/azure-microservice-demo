using Microsoft.EntityFrameworkCore;

namespace Demo.Provider
{
    public class ProviderDbContext : DbContext
    {
        public ProviderDbContext(DbContextOptions<ProviderDbContext> options) : base(options)
        {
        }

        public DbSet<Model.Provider> Providers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProviderDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
