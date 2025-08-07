using WalletTracker.Dtos;

namespace WalletTracker.Hasher;

public interface IAuthService
{
    Task Register(string userName, string email, string password);
    Task<string> Login(string email, string password);

}