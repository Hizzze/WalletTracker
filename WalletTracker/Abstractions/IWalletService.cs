using WalletTracker.Models;

namespace WalletTracker.Abstractions;

public interface IWalletService
{
    Task<Wallet> GetWalletById(Guid walletId, Guid userId);
    Task<List<Wallet>> GetUserWalletsAsync(Guid userId);
    Task<Wallet> CreateWalletAsync(Wallet wallet);
    Task UpdateWalletAsync(Wallet wallet, Guid userId);
    Task DeleteWalletAsync(Guid walletId, Guid userId);
}