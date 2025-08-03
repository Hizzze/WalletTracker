namespace WalletTracker.Models;

public class SavingsGoals
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double Target { get; set; }
    public double Current { get; set; }
    public DateTime TargetDate { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
}