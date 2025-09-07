using WalletTracker.Abstractions;
using WalletTracker.Dtos;
using WalletTracker.Exceptions;
using WalletTracker.Models;

namespace WalletTracker.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletService _walletService;
    private readonly ILogger<TransactionService> _logger;
    
    public TransactionService(ITransactionRepository transactionRepository, ILogger<TransactionService> logger, IWalletService walletService)
    {
        _transactionRepository = transactionRepository;
        _walletService = walletService;
        _logger = logger;
    }

    public async Task<Transaction> GetTransactionById(Guid id)
    {
        try
        {
            return await _transactionRepository.GetTransactionById(id);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Transaction with id {Id} not found", id);
            throw;
        }
    }

    public async Task<List<Transaction>> GetTransactionsByWalletId(Guid walletId)
    {
        try
        {
            return await _transactionRepository.GetTransactionsByWalletId(walletId);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Wallet with id {WalletId} not found", walletId);
            throw;
        }
    }

    public async Task<Transaction> CreateTransaction(TransactionCreateDto transactionDto)
    {
        try
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = transactionDto.Amount,
                Date = DateTime.UtcNow,
                Type = transactionDto.Type,
                Description = transactionDto.Description,
                WalletId = transactionDto.WalletId,
                // CategoryId = transactionDto.CategoryId
            };
            
            return await _transactionRepository.CreateTransaction(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction");
            throw;
        }
    }
    
    public async Task<Transaction> UpdateAsync(TransactionUpdateDto transactionDto)
    {
        try
        {
            var existingTransaction = await _transactionRepository.GetTransactionById(transactionDto.Id);
            
            // Обновляем только разрешенные поля
            existingTransaction.Amount = transactionDto.Amount;
            existingTransaction.Description = transactionDto.Description;
            existingTransaction.Date = transactionDto.Date;
            existingTransaction.Type = transactionDto.Type;
             await _transactionRepository.UpdateTransaction(existingTransaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating transaction {TransactionId}", transactionDto.Id);
            throw;
        }
        
        return await _transactionRepository.GetTransactionById(transactionDto.Id);
    }
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _transactionRepository.DeleteTransaction(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting transaction {TransactionId}", id);
            throw;
        }
    }
}