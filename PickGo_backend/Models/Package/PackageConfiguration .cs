using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;
using System;

namespace PickGo_backend.Models
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(p => p.Id);

            // Package → Request (Many Packages per Request)
            builder.HasOne(p => p.Request)
                   .WithMany(r => r.Packages)
                   .HasForeignKey(p => p.RequestId).OnDelete(DeleteBehavior.Restrict);
            ;

            // Package → Customer (Many Packages per Customer)
            builder.HasOne(p => p.Customer)
                   .WithMany(c => c.Packages)
                   .HasForeignKey(p => p.CustomerID).OnDelete(DeleteBehavior.NoAction);
            ;

            // Package → Courier (Many Packages per Courier, optional)
            builder.HasOne(p => p.Courier)
                   .WithMany(c => c.Packages)
                   .HasForeignKey(p => p.CourierID)
                   .OnDelete(DeleteBehavior.SetNull);

            // Enum configuration for Status (optional)
            builder.Property(p => p.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20);
        }
    }
    
}
