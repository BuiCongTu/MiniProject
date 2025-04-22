using Server.DTOs;

namespace Server.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(string userId);
    Task<UserDto> GetByUsernameAsync(string username);
    Task<UserDto> AddAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateAsync(UserDto userDto);
    Task<bool> ActiveAsync(string userId);
    Task<UserDto> Authenticate(LoginDto loginDto);
    
}