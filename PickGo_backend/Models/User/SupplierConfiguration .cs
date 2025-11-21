using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;

namespace PickGo_backend.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.UserId)
                   .IsRequired();

            builder.Property(s => s.IsDeleted)
                   .IsRequired();

            // One-to-one with User
            builder.HasOne(s => s.User)
                   .WithOne(u => u.Supplier)
                   .HasForeignKey<Supplier>(s => s.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
