using Microsoft.EntityFrameworkCore;

namespace Server.Models;

public class DatabaseDbContext: DbContext
{
    public DatabaseDbContext(DbContextOptions options): base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().HasData(
            new User()
            {
                UserId = "u1",
                Username = "admin",
                PasswordHash = "$2a$12$IJffGGP5Y427cT/TWDtbguji1qdzDI2wzl2asCWjqII4A9m27QXsa",
                Role = "Admin",
                TotalAmount = 0
            },
            new User()
            {
                UserId = "u2",
                Username = "user1",
                PasswordHash = "$2a$12$IJffGGP5Y427cT/TWDtbguji1qdzDI2wzl2asCWjqII4A9m27QXsa",
                Role = "User",
                TotalAmount = 0
            },
            new User()
            {
                UserId = "u3",
                Username = "user2",
                // PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                PasswordHash = "$2a$12$IJffGGP5Y427cT/TWDtbguji1qdzDI2wzl2asCWjqII4A9m27QXsa",
                Role = "User",
                TotalAmount = 0
            }
        );
    }
}