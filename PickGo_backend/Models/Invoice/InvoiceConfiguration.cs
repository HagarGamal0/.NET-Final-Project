using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;

namespace PickGo_backend.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Id);

            // Invoice → Package (One-to-One)
            builder.HasOne(i => i.Package)
                   .WithOne(p => p.Invoice)
                   .HasForeignKey<Invoice>(i => i.PackageID);
        }
    }
}
