using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Repositories;

public class UserRepository: IUserRepository
{
    private readonly DatabaseDbContext _context;

    public UserRepository(DatabaseDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(string userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> ActiveAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
    public async Task<User> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return user;
        }
        return null;
    }
}