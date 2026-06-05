using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PickGo_backend.Models
{
    public class CourierTransactionConfiguration : IEntityTypeConfiguration<CourierTransaction>
    {
        public void Configure(EntityTypeBuilder<CourierTransaction> builder)
        {
            builder.HasKey(ct => ct.Id);

            // Courier → many Transactions
            builder.HasOne(ct => ct.Courier)
                   .WithMany(c => c.Transactions)
                   .HasForeignKey(ct => ct.CourierID).OnDelete(DeleteBehavior.NoAction); // <-- fix
            ;

            // Package → many Transactions
            builder.HasOne(ct => ct.Package)
                   .WithMany(p => p.Transactions)
                   .HasForeignKey(ct => ct.PackageID).OnDelete(DeleteBehavior.NoAction); // <-- fix
            ;
        }
    }
}
