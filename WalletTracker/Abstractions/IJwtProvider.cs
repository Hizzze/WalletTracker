using WalletTracker.Models;

namespace WalletTracker.Abstractions;

public interface IJwtProvider
{
    string GenerateJwtToken(User user);
}