namespace Server.DTOs;

public class CreateUserDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public decimal TotalAmount { get; set; }
}