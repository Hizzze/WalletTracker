namespace WalletTracker.Models;

public class Budget
{
    public Guid Id { get; set; }
    public double Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }
    public Category? Category { get; set; }
    public User? User { get; set; }
}