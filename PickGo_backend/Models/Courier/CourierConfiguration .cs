using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PickGo_backend.Models
{
    public class CourierConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            // Optional properties
            builder.Property(c => c.LicensePhotoBack).IsRequired(false);
            builder.Property(c => c.LicensePhotoFront).IsRequired(false);
            builder.Property(c => c.VehcelLicensePhotoFront).IsRequired(false);
            builder.Property(c => c.VehcelLicensePhotoBack).IsRequired(false);
            builder.Property(c => c.PhotoUrl).IsRequired(false);
            builder.Property(c => c.RejectionReason).IsRequired(false);

            // Primary key
            builder.HasKey(c => c.Id);

            // One-to-one with User
            builder.HasOne(c => c.User)
                   .WithOne(u => u.Courier)
                   .HasForeignKey<Courier>(c => c.UserId);

            // One-to-many: Courier → Packages
            builder.HasMany(c => c.Packages)
                   .WithOne(p => p.Courier)
                   .HasForeignKey(p => p.CourierID)
                   .OnDelete(DeleteBehavior.SetNull);

           

            // One-to-many: Courier → Locations
            builder.HasMany(c => c.Locations)
                   .WithOne(l => l.Courier)
                   .HasForeignKey(l => l.CourierID);

            // One-to-many: Courier → Transactions
            builder.HasMany(c => c.Transactions)
                   .WithOne(t => t.Courier)
                   .HasForeignKey(t => t.CourierID);

            // One-to-many: Courier → Subscriptions
            builder.HasMany(c => c.CourierSubscriptions)
                   .WithOne(cs => cs.Courier)
                   .HasForeignKey(cs => cs.CourierId);


            builder.HasOne(c => c.CurrentSubscription)
       .WithOne()
       .HasForeignKey<Courier>(c => c.CurrentSubscriptionId)
       .OnDelete(DeleteBehavior.SetNull);

            // One-to-many: Courier → DeliveryProofs
            builder.HasMany(c => c.DeliveryProofs)
                   .WithOne(dp => dp.Courier)
                   .HasForeignKey(dp => dp.CourierID);
        }
    }
}
