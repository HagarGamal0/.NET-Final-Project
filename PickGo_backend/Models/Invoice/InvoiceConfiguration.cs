using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;
using PickGo_backend.Models.Enums;

namespace PickGo_backend.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Id); // Primary key from BaseModel

            builder.Property(i => i.Cost)
                   .IsRequired();

            // Store enum as string in DB
            builder.Property(i => i.PaymentType)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(i => i.InvoiceNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            // Relation: Invoice -> Request
            builder.HasOne(i => i.Request)
                  .WithOne(r => r.Invoice)           // Each Request has one Invoice
                  .HasForeignKey<Invoice>(i => i.RequestID) // Specify that FK is in Invoice
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
