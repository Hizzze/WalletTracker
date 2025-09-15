using WalletTracker.Dtos;
using WalletTracker.Models;
using WalletTracker.Services;

namespace WalletTracker.Abstractions;

public interface IUserService
{
    public Task<User> GetUserByIdAsync(Guid id);
    public Task<User> GetUserByEmailAsync(string email);
    public Task<List<User>> GetAllUsersAsync();
    public Task<User> ExistUserByEmailAsync(string email);
    public Task<User> CreateUserAsync(CreateUserDto userDto);
    public Task<User> UpdateUser(Guid id, UpdateUserDto userDto);
    public Task<User> DeleteUserAsync(Guid id);
}