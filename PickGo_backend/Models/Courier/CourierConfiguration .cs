using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PickGo_backend.Models
{
    public class CourierConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.User)
                   .WithOne(u => u.Courier);

        }
    }
}
