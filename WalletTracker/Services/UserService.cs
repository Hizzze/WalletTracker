using WalletTracker.Abstractions;
using WalletTracker.Dtos;
using WalletTracker.Exceptions;
using WalletTracker.Hasher;
using WalletTracker.Models;

namespace WalletTracker.Services;

public class UserService : IUserService
{
    
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<UserService> _logger;
    
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }


    public async Task<User> GetUserByIdAsync(Guid id)
    {
        try
        {
          return await _userRepository.GetByIdAsync(id);
        }
        catch (NotFoundException e)
        {
            _logger.LogError(e, "Error getting user by id");
            throw;
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
        catch (NotFoundException e)
        {
            _logger.LogError(e, "Error getting user by email");
            throw;
        }
    }

    public async Task<User> ExistUserByEmailAsync(string email)
    {
        try
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
        catch (NotFoundException e)
        {
            _logger.LogError(e, "Error getting user by email");
            throw;
        }
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            return await _userRepository.GetAllUsersAsync();
        }
        catch (NotFoundException e)
        {
            _logger.LogError(e, "Error getting all users");
            throw;
        }
    }
    


    public async Task<User> CreateUserAsync(CreateUserDto userDto)
    {
        try
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = _passwordHasher.Generate(userDto.Password),
                CreatedAt = DateTime.UtcNow
            };
            
           return await _userRepository.CreateUserAsync(user);
        }
        catch (NotFoundException e)
        {
            _logger.LogError(e, "Error creating user");
            throw;
        }
    }

    public async Task<User> UpdateUser(Guid id, UpdateUserDto userDto)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            return await _userRepository.UpdateUserAsync(id, userDto);
        }
        catch (NotFoundException e)
        {
            _logger.LogError(e, "Error updating user");
            throw;
        }
    }

    public async Task<User> DeleteUserAsync(Guid id)
    {
        try
        {
            return await _userRepository.DeleteUserAsync(id);
        }
        catch (NotFoundException e)
        {
            _logger.LogError(e, "Error deleting user");
            throw;
        }
    }
    
    
}