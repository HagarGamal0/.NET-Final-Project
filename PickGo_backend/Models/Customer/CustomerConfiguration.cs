using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;
namespace PickGo_backend
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        { 
                builder.HasKey(c => c.Id);

                builder.HasOne(c => c.User)
                       .WithOne(u => u.Customer)
                       .HasForeignKey<Customer>(c => c.UserId);
            }
    }
}
