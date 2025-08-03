using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletTracker.Models;

namespace WalletTracker.Configurations;

public class SavingsGoalsConfiguration : IEntityTypeConfiguration<SavingsGoals>
{
    public void Configure(EntityTypeBuilder<SavingsGoals> builder)
    {
        builder.ToTable("SavingsGoals");
        builder.HasKey(x => x.Id);
        
        // Fields
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Target)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        builder.Property(x => x.Current)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        builder.Property(x => x.TargetDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");
            
    }
}