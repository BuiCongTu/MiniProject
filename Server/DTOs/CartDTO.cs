namespace Server.DTOs;

public class CartDTO
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public string? ProductName { get; set; }
    public string? Price { get; set; }

}