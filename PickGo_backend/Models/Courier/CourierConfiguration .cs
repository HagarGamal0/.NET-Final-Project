using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PickGo_backend.Models
{
    public class CourierConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder.Property(c => c.LicensePhotoBack).IsRequired(false);
            builder.Property(c => c.LicensePhotoFront).IsRequired(false);

            builder.Property(c => c.VehcelLicensePhotoFront).IsRequired(false);
            builder.Property(c => c.PhotoUrl).IsRequired(false);
            builder.Property(c => c.VehcelLicensePhotoBack).IsRequired(false);

            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.User)
                   .WithOne(u => u.Courier);

        }
    }
}
