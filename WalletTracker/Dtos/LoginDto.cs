using System.ComponentModel.DataAnnotations;

namespace WalletTracker.Dtos;

public record LoginDto([Required, EmailAddress]string? Email,[Required] string? Password);
