using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;

namespace PickGo_backend
{
    public class DeliveryProofConfiguration : IEntityTypeConfiguration<DeliveryProof>
    {
        public void Configure(EntityTypeBuilder<DeliveryProof> builder)
        {
            // Primary Key
            builder.HasKey(dp => dp.Id);

        // DeliveryProof → Package (One-to-One)  
        builder.HasOne(dp => dp.Package)
               .WithOne(p => p.DeliveryProof)
               .HasForeignKey<DeliveryProof>(dp => dp.PackageID)
               .OnDelete(DeleteBehavior.Cascade); // cascade delete for Package  

            // DeliveryProof → Customer (optional, many DeliveryProofs per Customer)  
            builder.HasOne(dp => dp.Customer)
                   .WithMany(c => c.DeliveryProofs)
                   .HasForeignKey(dp => dp.CustomerID)
                   .OnDelete(DeleteBehavior.SetNull); // nullify CustomerID if Customer deleted  

            // DeliveryProof → Courier (optional, many DeliveryProofs per Courier)  
            builder.HasOne(dp => dp.Courier)
                   .WithMany(c => c.DeliveryProofs)
                   .HasForeignKey(dp => dp.CourierID)
                   .OnDelete(DeleteBehavior.NoAction); // prevent multiple cascade paths  

            // Optional: set column properties (if needed)  
            builder.Property(dp => dp.SignatureUrl).HasMaxLength(500).IsRequired(false);
            builder.Property(dp => dp.CustomerOTP).HasMaxLength(100).IsRequired(false);
            builder.Property(dp => dp.DeliveredAt).IsRequired(false);
        }
    }  

}
