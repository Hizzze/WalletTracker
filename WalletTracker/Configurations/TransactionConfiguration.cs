using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletTracker.Models;

namespace WalletTracker.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(x => x.Id);
        
        // Fields
        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(x => x.Date)
            .IsRequired();
        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<string>();
        
        // Relations
        builder.HasOne(x => x.Wallet)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}