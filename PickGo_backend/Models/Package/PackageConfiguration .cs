using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;
using System;

namespace PickGo_backend.Configurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(p => p.Id); // Primary key from BaseModel

            builder.Property(p => p.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(p => p.Weight)
                   .IsRequired();

            builder.Property(p => p.Fragile)
                   .IsRequired();

            builder.Property(p => p.ExpireDate)
                   .IsRequired();

            builder.Property(p => p.ShipmentCost)
                   .IsRequired();

            builder.Property(p => p.InvoiveImage)
                   .HasMaxLength(250);

            // Many Packages -> One Request
            builder.HasOne(p => p.Request)
                   .WithMany(r => r.Packages)   // Request should have ICollection<Package> Packages
                   .HasForeignKey(p => p.RequestID)
                   .OnDelete(DeleteBehavior.Cascade); // Delete packages if request is deleted
        }
    }
}
