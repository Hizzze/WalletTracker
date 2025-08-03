using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletTracker.Models;

namespace WalletTracker.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.ToTable("Users");
      builder.HasKey(x => x.Id);
      
      // Fields
      builder.Property(x => x.Username)
          .IsRequired()
          .HasMaxLength(50);
      builder.Property(x => x.Email)
          .IsRequired()
          .HasMaxLength(100);
      builder.Property(x => x.PasswordHash)
          .IsRequired();
      builder.Property(x => x.CreatedAt)
          .IsRequired()
          .HasDefaultValueSql("now()");
      
      // Relations
      builder.HasMany(x => x.Wallets)
          .WithOne(x => x.User)
          .HasForeignKey(x => x.UserId)
          .OnDelete(DeleteBehavior.Cascade);
      
      builder.HasMany(x => x.Categories)
          .WithOne(x => x.User)
          .HasForeignKey(x => x.UserId)
          .OnDelete(DeleteBehavior.Cascade);
    }
}