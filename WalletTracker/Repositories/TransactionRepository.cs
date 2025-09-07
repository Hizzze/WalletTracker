using Microsoft.EntityFrameworkCore;
using WalletTracker.Abstractions;
using WalletTracker.Database;
using WalletTracker.Exceptions;
using WalletTracker.Models;

namespace WalletTracker.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly WalletTrackerDbContext _context;
    private readonly ILogger<TransactionRepository> _logger;
    
    public TransactionRepository(WalletTrackerDbContext context, ILogger<TransactionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Transaction> GetTransactionById(Guid id)
    {
            var transaction = await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            return transaction ?? throw new NotFoundException("Transaction not found");
    }

    public async Task<List<Transaction>> GetTransactionsByWalletId(Guid walletId)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(x => x.WalletId == walletId).ToListAsync();
    }
    
    public async Task<Transaction> CreateTransaction(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task UpdateTransaction(Transaction transaction)
    {
        var existingTransaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == transaction.Id);
        if (existingTransaction == null)        
        {
            _logger.LogWarning("Transaction with id {Id} not found for update", transaction.Id);
            return;
        }
        existingTransaction.Amount = transaction.Amount;
        existingTransaction.Description = transaction.Description;
        existingTransaction.Category = transaction.Category;
        existingTransaction.Date = transaction.Date;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTransaction(Guid id)
    {
        var deleteTransaction = await _context.Transactions.Where(x => x.Id == id).ExecuteDeleteAsync();

        if (deleteTransaction == 0)
        {
            _logger.LogWarning("Transaction with id {Id} not found for delete", id);
            throw new NotFoundException("Transaction not found");
        }
    }
}