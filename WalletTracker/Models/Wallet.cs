namespace WalletTracker.Models;

public class Wallet
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string? Currency { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public List<Transaction>? Transactions { get; set; }
}