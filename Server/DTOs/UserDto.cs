using System.ComponentModel.DataAnnotations;

namespace Server.DTOs;

public class UserDto
{
    [Key] public string UserId { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public decimal TotalAmount { get; set; }
}