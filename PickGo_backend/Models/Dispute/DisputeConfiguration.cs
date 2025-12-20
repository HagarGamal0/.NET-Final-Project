using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickGo_backend.Models;

public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
{
    public void Configure(EntityTypeBuilder<Dispute> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasOne(d => d.Package)
               .WithMany(p => p.Disputes)
               .HasForeignKey(d => d.PackageId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.ProofImages)
               .WithOne(p => p.Dispute)
               .HasForeignKey(p => p.DisputeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.StatusHistory)
               .WithOne(s => s.Dispute)
               .HasForeignKey(s => s.DisputeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
