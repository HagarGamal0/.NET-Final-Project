using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;

public class SupplierSubscriptionConfiguration : IEntityTypeConfiguration<SupplierSubscription>
{
    public void Configure(EntityTypeBuilder<SupplierSubscription> builder)
    {
        builder.HasKey(ss => new { ss.SupplierId, ss.SubscriptionId });

        builder.HasOne(ss => ss.Supplier)
               .WithMany(s => s.SupplierSubscriptions)
               .HasForeignKey(ss => ss.SupplierId);

        builder.HasOne(ss => ss.Subscription)
               .WithMany(s => s.SupplierSubscriptions)
               .HasForeignKey(ss => ss.SubscriptionId);
    }
}