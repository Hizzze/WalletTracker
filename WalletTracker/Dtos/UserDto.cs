namespace WalletTracker.Dtos;

public record CreateUserDto(string UserName, string Email, string Password);
public record UpdateUserDto(string UserName, string Email);
