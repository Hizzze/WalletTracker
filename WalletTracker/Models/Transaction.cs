namespace WalletTracker.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public TransactionType Type { get; set; }
    
    public Guid WalletId { get; set; }
    public Wallet? Wallet { get; set; }
}

public enum TransactionType
{
    Income,
    Expense
}