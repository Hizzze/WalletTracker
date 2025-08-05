using Microsoft.EntityFrameworkCore;
using WalletTracker.Abstractions;
using WalletTracker.Database;
using WalletTracker.Models;
using ILogger = Serilog.ILogger;

namespace WalletTracker.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly WalletTrackerDbContext _context;
    private readonly ILogger<WalletRepository> _logger;
    
    public WalletRepository(WalletTrackerDbContext context, ILogger<WalletRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Wallet?> GetByIdAsync(Guid id)
    {
        return await _context.Wallets
            .Include(x => x.Transactions)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Wallet>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Wallets
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task CreateWallet(Wallet wallet){
        await _context.Wallets.AddAsync(wallet);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateWallet(Wallet wallet)
    {
        var existingWallet = await _context.Wallets.FirstOrDefaultAsync(x => x.Id == wallet.Id);
        if (existingWallet == null)
        {
            _logger.LogWarning("Wallet with id {Id} not found for update", wallet.Id);
           return;
        }
        _context.Entry(existingWallet).CurrentValues.SetValues(wallet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteWallet(Guid id)
    {
        await _context.Wallets.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}