using Server.Models;

namespace Server.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByIdAsync(string userId);
    Task<User> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> ActiveAsync(string userId);
    Task<User> AuthenticateAsync(string username, string password);
}