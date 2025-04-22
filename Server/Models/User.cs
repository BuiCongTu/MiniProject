using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class User
{
    [Key] public string UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public decimal TotalAmount { get; set; }
}