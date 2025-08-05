using System.ComponentModel.DataAnnotations;
using WalletTracker.Abstractions;
using WalletTracker.Exceptions;
using WalletTracker.Models;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace WalletTracker.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly ILogger<WalletService> _logger;
    private const int MAX_WALLETS_PER_USER = 5;
    
    public WalletService(IWalletRepository walletRepository, ILogger<WalletService> logger)
    {
        _walletRepository = walletRepository;
        _logger = logger;
    }

    public async Task<Wallet> GetWalletById(Guid walletId, Guid userId)
    {
        var wallet = await _walletRepository.GetByIdAsync(walletId);
        if (wallet == null || wallet.UserId != userId)
        {
            _logger.LogWarning("Wallet {WalletId} not found or access denied for user {UserId}", walletId, userId);
            throw new Exception("Wallet not found or access denied");
        }
        return wallet;
    }

    public async Task<List<Wallet>> GetUserWalletsAsync(Guid userId)
    {
        return await _walletRepository.GetByUserIdAsync(userId);
    }

    public async Task<Wallet> CreateWalletAsync(Wallet wallet)
    {
        if (wallet.Balance < 0)
        {
            _logger.LogWarning("Attempt to create wallet with negative balance: {Balance}", wallet.Balance);
            throw new ValidationException("Wallet balance cannot be negative !");
        }

        var userWallets =  await _walletRepository.GetByUserIdAsync(wallet.UserId);
        if (userWallets.Count >= MAX_WALLETS_PER_USER)
        {
            _logger.LogWarning("User {UserId} has reached the wallet limit ({MaxWallets})", wallet.UserId, MAX_WALLETS_PER_USER);
            throw new BusinessRuleException($"Maximum number of wallets ({MAX_WALLETS_PER_USER}) reached");
        }
        await _walletRepository.CreateWallet(wallet);
        _logger.LogInformation("Wallet {WalletId} created for user {UserId}", wallet.Id, wallet.UserId);
        return wallet;
    }

    public async Task UpdateWalletAsync(Wallet wallet, Guid userId)
    {
        var existingWallet = await GetWalletById(wallet.Id, userId);

        if (wallet.Balance < 0)
        {
            _logger.LogWarning("Attempt to set negative balance for wallet {WalletId}", wallet.Id);
            throw new ValidationException("Wallet balance cannot be negative!");
        }

        existingWallet.Name = wallet.Name;
        existingWallet.Balance = wallet.Balance;
        existingWallet.Currency = wallet.Currency;

        await _walletRepository.UpdateWallet(existingWallet);
        _logger.LogInformation("Wallet {WalletId} updated by user {UserId}", wallet.Id, userId);
    }

    public async Task DeleteWalletAsync(Guid walletId, Guid userId)
    {
         await _walletRepository.DeleteWallet(walletId);
        _logger.LogInformation("Wallet {WalletId} deleted by user {UserId}", walletId, userId);
    }
}