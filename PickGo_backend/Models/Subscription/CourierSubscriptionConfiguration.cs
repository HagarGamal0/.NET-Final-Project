using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;

public class CourierSubscriptionConfiguration : IEntityTypeConfiguration<CourierSubscription>
{
    public void Configure(EntityTypeBuilder<CourierSubscription> builder)
    {
        builder.HasKey(cs => new { cs.CourierId, cs.SubscriptionId });

        builder.HasOne(cs => cs.Courier)
               .WithMany(c => c.CourierSubscriptions)
               .HasForeignKey(cs => cs.CourierId);

        builder.HasOne(cs => cs.Subscription)
               .WithMany(s => s.CourierSubscriptions)
               .HasForeignKey(cs => cs.SubscriptionId);
    }
}