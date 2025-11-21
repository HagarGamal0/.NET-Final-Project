using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;

namespace PickGo_backend.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.CreatedAt).IsRequired();
            builder.Property(r => r.Source).IsRequired().HasMaxLength(200);
            builder.Property(r => r.Destination).IsRequired().HasMaxLength(200);
            builder.Property(r => r.ShipmentCost).IsRequired().HasMaxLength(50);

            // Relation with User (many Requests -> one User)
            builder.HasOne(r => r.User)
                   .WithMany() // You can add ICollection<Request> in User if needed
                   .HasForeignKey(r => r.UserID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
