using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ShipmentReviewConfiguration : IEntityTypeConfiguration<ShipmentReview>
{
    public void Configure(EntityTypeBuilder<ShipmentReview> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasOne(r => r.Package)
               .WithOne(p => p.ShipmentReview)
               .HasForeignKey<ShipmentReview>(r => r.PackageID)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
