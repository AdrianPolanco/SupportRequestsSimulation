using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simulation.Entities;

namespace Simulation.Persistence.Configurations;

public class SupportRequestConfiguration: IEntityTypeConfiguration<SupportRequest>
{
    public void Configure(EntityTypeBuilder<SupportRequest> builder)
    {
        builder.ToTable("support_requests");
        builder.HasKey(sr => sr.Id);
        builder.Property(sr => sr.Subject).HasColumnName("subject").HasMaxLength(100).IsRequired();
        builder.Property(sr => sr.Description).HasColumnName("description").HasMaxLength(250).IsRequired();
        builder.Property(sr => sr.Priority).HasColumnName("priority")
            .HasConversion<string>(p => p.ToString(), 
            p => Enum.Parse<Priority>(p)).IsRequired();

        builder.Property(sr => sr.Status).HasColumnName("status")
            .HasConversion<string>(s => s.ToString(),
                s => Enum.Parse<Status>(s)).IsRequired();

        builder.Property(sr => sr.CustomerId).HasColumnName("customerId");

        builder.Property(sr => sr.CreatedAt).HasColumnName("created_at");

        builder.HasOne(sr => sr.Customer)
            .WithMany()
            .HasForeignKey(sr => sr.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}