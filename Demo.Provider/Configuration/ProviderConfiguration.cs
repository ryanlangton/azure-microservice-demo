using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Provider.Configuration
{
    public class ProviderConfiguration : IEntityTypeConfiguration<Model.Provider>
    {
        public void Configure(EntityTypeBuilder<Model.Provider> builder)
        {
            builder.Property(e => e.EmailAddress)
                .HasMaxLength(150);
            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(12);
        }
    }
}
