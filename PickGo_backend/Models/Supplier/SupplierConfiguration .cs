using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PickGo_backend.Models
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(s => s.Id);

            // Supplier → User (One-to-One)
            builder.HasOne(s => s.User)
                   .WithOne(u => u.Supplier)
                   .HasForeignKey<Supplier>(s => s.UserId);

        }
    }
}

