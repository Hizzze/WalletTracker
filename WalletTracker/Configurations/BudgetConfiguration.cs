using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletTracker.Models;

namespace WalletTracker.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("Budgets");
        builder.HasKey(x => x.Id);
        
        // Fields
        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.Property(x => x.StartDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");
            
        builder.Property(x => x.EndDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");
        
        // Relations
        builder.HasOne(x => x.User)
            .WithMany(u => u.Budgets)  // Изменено на WithMany
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}