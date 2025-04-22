using Server.DTOs;
using Server.Models;
using Server.Repositories;

namespace Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Role = u.Role,
                TotalAmount = u.TotalAmount
            }).ToList();
        }

        public async Task<UserDto> GetByIdAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role,
                TotalAmount = user.TotalAmount
            };
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role,
                TotalAmount = user.TotalAmount
            };
        }

        public async Task<UserDto> AddAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                Username = createUserDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                Role = createUserDto.Role,
                TotalAmount = createUserDto.TotalAmount
            };
            var addedUser = await _userRepository.AddAsync(user);
            return new UserDto
            {
                UserId = addedUser.UserId,
                Username = addedUser.Username,
                Role = addedUser.Role,
                TotalAmount = addedUser.TotalAmount
            };
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(userDto.UserId);
            if (existingUser == null) return null;

            existingUser.Username = userDto.Username;
            existingUser.Role = userDto.Role;
            existingUser.TotalAmount = userDto.TotalAmount;

            await _userRepository.UpdateAsync(existingUser);
            return new UserDto
            {
                UserId = existingUser.UserId,
                Username = existingUser.Username,
                Role = existingUser.Role,
                TotalAmount = existingUser.TotalAmount
            };
        }

        public async Task<bool> ActiveAsync(string userId)
        {
            return await _userRepository.ActiveAsync(userId);
        }

        public async Task<UserDto> Authenticate(LoginDto loginDto)
        {
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Incorrect password.");
            }
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role,
                TotalAmount = user.TotalAmount
            };
        }
    }
}