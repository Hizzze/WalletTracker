using WalletTracker.Models;

namespace WalletTracker.Abstractions;

public interface ITransactionRepository
{
    Task<Transaction> GetTransactionById(Guid id);
    Task<List<Transaction>> GetTransactionsByWalletId(Guid walletId);
    Task<Transaction> CreateTransaction(Transaction transaction);
    Task UpdateTransaction(Transaction transaction);
    Task DeleteTransaction(Guid id);
}