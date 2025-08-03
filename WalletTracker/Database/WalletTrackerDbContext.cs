using Microsoft.EntityFrameworkCore;
using WalletTracker.Models;

namespace WalletTracker.Database;

public class WalletTrackerDbContext : DbContext
{
    public WalletTrackerDbContext(DbContextOptions<WalletTrackerDbContext> options) : base(options)
    {
        
    } 
    
    public DbSet<User> Users { get; set; } 
    public DbSet<SavingsGoals> SavingsGoals { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Budget> Budgets { get; set; }

    // Apply Configurations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletTrackerDbContext).Assembly);
    }
}