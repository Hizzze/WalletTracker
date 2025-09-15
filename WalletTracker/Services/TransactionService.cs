using AutoMapper;
using WalletTracker.Abstractions;
using WalletTracker.AutoMapperProfile;
using WalletTracker.Dtos;
using WalletTracker.Exceptions;
using WalletTracker.Models;

namespace WalletTracker.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletService _walletService;
    private readonly ILogger<TransactionService> _logger;
    private readonly IMapper _mapper;
    
    public TransactionService(ITransactionRepository transactionRepository, ILogger<TransactionService> logger, IWalletService walletService, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _walletService = walletService;
        _logger = logger;
        _mapper = mapper;
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
            var transaction = _mapper.Map<Transaction>(transactionDto);
            
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

            if (existingTransaction == null)
            {
                _logger.LogWarning("Transaction with id {Id} not found for update", transactionDto.Id);
                throw new NotFoundException("Transaction not found");
            }
            
            _mapper.Map(transactionDto, existingTransaction);

            await _transactionRepository.UpdateTransaction(existingTransaction);

            return existingTransaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating transaction {TransactionId}", transactionDto.Id);
            throw;
        }
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