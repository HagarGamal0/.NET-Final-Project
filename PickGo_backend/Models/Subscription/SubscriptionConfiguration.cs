using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PickGo_backend.Models
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {

        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            // Set precision for Price
            builder.Property(s => s.Price)
                   .HasPrecision(18, 4); // 18 digits total, 4 decimals

            // Optional: set max lengths for strings if needed
            builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
            builder.Property(s => s.Description).HasMaxLength(500).IsRequired();
            builder.Property(s => s.UserType).HasMaxLength(50).IsRequired();
        }
    }
}
