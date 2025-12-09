using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PickGo_backend.Models
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.Property(r => r.PickupAddress)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(r => r.DropoffAddress)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(r => r.ReceiverName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(r => r.ReceiverPhone)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(r => r.ItemsDescription)
                   .HasMaxLength(1000);

            builder.Property(r => r.Notes)
                   .HasMaxLength(500);

            builder.Property(r => r.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            // Relationship: 1 Request → Many Packages
            builder.HasMany(r => r.Packages)
                   .WithOne(p => p.Request)
                   .HasForeignKey(p => p.RequestId);
        }
    }
}
