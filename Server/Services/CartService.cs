using Server.DTOs;
using Server.Repositories;

namespace Server.Services;
public class CartService : ICartService
{
    private readonly ICartRepository cartRepo;
    public CartService(ICartRepository cartRepo)
    {
        this.cartRepo = cartRepo;
    }
    
    public async Task<IEnumerable<CartDTO>> GetCartByUser(string userId)
    {
        var cart = await cartRepo.GetCartByUser(userId);
        return cart.Select(c => new CartDTO
        {
            Id = c.Id,
            UserId = c.UserId,
            ProductId = c.ProductId,
            Quantity = c.Quantity
            }).ToList();
    }
    
    public async Task<CartDTO> AddToCart(CartDTO cartDto)
    {
        var cart = new CartDTO()
        {
            UserId = cartDto.UserId,
            ProductId = cartDto.ProductId,
            Quantity = cartDto.Quantity
        };
        return cart;
    }
    
    public async Task Payment(string userId)
    {
        await cartRepo.Payment(userId);
    }
}