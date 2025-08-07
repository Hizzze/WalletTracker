using WalletTracker.Abstractions;
using WalletTracker.Dtos;
using WalletTracker.Hasher;
using WalletTracker.Models;

namespace WalletTracker.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,IJwtProvider provider, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = provider;
        _logger = logger;
    }

    public async Task Register(string userName, string email, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = userName,
            Email = email,
            PasswordHash = hashedPassword
        };
        await _userRepository.CreateUserAsync(user);
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (!result)
        {
            _logger.LogWarning("Login failed for user {Email}", email);
            throw new UnauthorizedAccessException("Login failed");
        }

        var token = _jwtProvider.GenerateJwtToken(user);

        return token;
    }
    
    
}