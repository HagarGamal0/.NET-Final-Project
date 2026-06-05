using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PickGo_backend.Models
{
    public class CourierLocationConfiguration : IEntityTypeConfiguration<CourierLocation>
    {
        public void Configure(EntityTypeBuilder<CourierLocation> builder)
        {
            builder.HasKey(cl => cl.Id);

            builder.HasOne(cl => cl.Courier)
                   .WithMany(c => c.Locations)
                   .HasForeignKey(cl => cl.CourierID).OnDelete(DeleteBehavior.NoAction); // <-- fix
            ;

        }
    }
}
