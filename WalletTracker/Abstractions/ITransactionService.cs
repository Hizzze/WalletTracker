using WalletTracker.Dtos;
using WalletTracker.Models;

namespace WalletTracker.Abstractions;

public interface ITransactionService
{
    Task<Transaction> GetTransactionById(Guid id);
    Task<List<Transaction>> GetTransactionsByWalletId(Guid walletId);
    Task<Transaction> CreateTransaction(TransactionCreateDto transactionDto);
    Task<Transaction> UpdateAsync(TransactionUpdateDto transactionDto);
    Task DeleteAsync(Guid id);
}