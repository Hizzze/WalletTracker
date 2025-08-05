using WalletTracker.Models;

namespace WalletTracker.Abstractions;

public interface IWalletRepository
{
    Task<Wallet?> GetByIdAsync(Guid id);
    Task<List<Wallet>> GetByUserIdAsync(Guid userId);
    Task CreateWallet(Wallet wallet);
    Task UpdateWallet(Wallet wallet);
    Task DeleteWallet(Guid id);
}