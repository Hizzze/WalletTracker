using System.ComponentModel.DataAnnotations;
using WalletTracker.Models;

namespace WalletTracker.Dtos;

public record TransactionCreateDto([Required]decimal Amount,[Required, MaxLength(100)] string Description, Guid WalletId, [Required]TransactionType Type);
public record TransactionUpdateDto([Required]Guid Id,[Required]decimal Amount,[Required, MaxLength(100)] string Description, DateTime Date, Guid WalletId, [Required]TransactionType Type);