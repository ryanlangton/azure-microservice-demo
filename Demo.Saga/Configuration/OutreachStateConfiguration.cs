using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QES.Demo.Saga.Model;

namespace QES.Demo.Saga.Configuration
{
    public class OutreachStateConfiguration : SagaClassMap<OutreachState>
    {
        protected override void Configure(EntityTypeBuilder<OutreachState> builder, ModelBuilder model)
        {
            builder.Property(e => e.CurrentState).HasMaxLength(64);
            builder.Property(e => e.StartDate)
                .IsRequired();
            builder.Property(e => e.CompleteDate);
            builder.Property(e => e.CorrelationId)
                .HasMaxLength(256)
                .IsRequired();
            builder.Property(e => e.EmailAddress)
                .HasMaxLength(150);
            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(12);
            builder.HasMany(e => e.OutreachEmailAttempts)
                .WithOne(e => e.OutreachState)
                .HasForeignKey(e => e.OutreachStateId);
            builder.HasMany(e => e.OutreachPhoneAttempts)
                .WithOne(e => e.OutreachState)
                .HasForeignKey(e => e.OutreachStateId);

        }
    }
}
