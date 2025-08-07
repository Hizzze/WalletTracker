using WalletTracker.Models;

namespace WalletTracker.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, bool includeRelations = false);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(User user);
    Task<User?> DeleteUserAsync(Guid id);
    Task<List<User>> GetAllUsersAsync();

}