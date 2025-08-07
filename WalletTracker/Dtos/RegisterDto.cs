using System.ComponentModel.DataAnnotations;

namespace WalletTracker.Dtos;

public record RegisterDto([Required, MinLength(3)]string? Username,[Required, EmailAddress] string? Email,[Required, MinLength(6)] string? Password);