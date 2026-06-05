using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;

namespace PickGo_backend.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id); // primary key

            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.Address)
                   .HasMaxLength(250);

            builder.Property(u => u.Gender)
                   .HasMaxLength(10);

            // Enforce unique indexes
            builder.HasIndex(u => u.UserName)
                   .IsUnique();

            builder.HasIndex(u => u.Email)
                   .IsUnique();
        }
    }
}
