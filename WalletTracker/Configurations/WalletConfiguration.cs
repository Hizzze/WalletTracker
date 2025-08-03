using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletTracker.Models;

namespace WalletTracker.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallets");
        
        builder.HasKey(x => x.Id);
        
        // Fields
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Balance)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3);
    }
}