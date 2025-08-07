using Microsoft.EntityFrameworkCore;
using WalletTracker.Abstractions;
using WalletTracker.Database;
using WalletTracker.Exceptions;
using WalletTracker.Models;

namespace WalletTracker.Repositories;

public class UserRepository : IUserRepository
{
    private readonly WalletTrackerDbContext _context;
    private readonly ILogger<UserRepository> _logger;
    
    public UserRepository(WalletTrackerDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User?> GetByIdAsync(Guid id, bool includeRelations = false)
    {
        var query = _context.Users.AsQueryable();
        if (includeRelations)
        {
            query = query
                .Include(u => u.Wallets)
                .Include(u => u.Budgets)
                .Include(u => u.Categories)
                .Include(u => u.SavingsGoals);
            
        }
        return await query.FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var checkEmail = await _context.Users.AnyAsync(x => x.Email == email);
        if (!checkEmail)
        {
            throw new NotFoundException("Email not found");
        }
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task ExistsByEmailAsync(string email)
    {
        await _context.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
    

    public async Task<User?> CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
        if (existingUser == null)
        {
            _logger.LogWarning("User with id {Id} not found for update", user.Id);
            throw new NotFoundException("User not found");
        }
        _context.Entry(existingUser).CurrentValues.SetValues(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            _logger.LogWarning("User with id {Id} not found for delete", id);
            throw new NotFoundException("User not found");
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }
}