using System.ComponentModel.DataAnnotations;

namespace WalletTracker.Dtos;


public record WalletDto([Required]string Name, [Range(0, double.MaxValue)]decimal Balance, [Required, StringLength(3)]string Currency);