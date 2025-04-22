using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetUserById(string UserId)
        {
            var user = await _userService.GetByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            if (createUserDto == null || string.IsNullOrWhiteSpace(createUserDto.Username) || string.IsNullOrWhiteSpace(createUserDto.Password))
            {
                return BadRequest("Username and password are required.");
            }
            var userDto = await _userService.AddAsync(createUserDto);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string UserId, UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (UserId != userDto.UserId)
            {
                return BadRequest("ID mismatch");
            }

            var updatedUser = await _userService.UpdateAsync(userDto);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActiveAsync(string UserId)
        {
            var result = await _userService.ActiveAsync(UserId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}