using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;

namespace PickGo_backend.Configurations
{
    public class ShipmentReviewConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasKey(r => r.Id);

            // Request → Supplier (Many Requests per Supplier)
            builder.HasOne(r => r.Supplier)
                   .WithMany(s => s.Requests);
          
        }
    }
}
