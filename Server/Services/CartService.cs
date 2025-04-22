using Server.DTOs;
using Server.Models;
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
            Quantity = c.Quantity,
            Price = c.Product?.Price.ToString(),
            ProductName = c.Product?.Name
        }).ToList();
    }
    
    public async Task<CartDTO> AddToCart(CartDTO cartDto)
    {
        var cart = new Cart()
        {
            UserId = cartDto.UserId,
            ProductId = cartDto.ProductId,
            Quantity = cartDto.Quantity
        };
        var addedItem = await cartRepo.AddToCart(cart);
        return cartDto;
    }
    
    public async Task Payment(string userId)
    {
        await cartRepo.Payment(userId);
    }

    public async Task<CartDTO> UpdateCart(CartDTO cartDto)
    {
        var cart = new Cart()
        {
            Id = cartDto.Id,
            UserId = cartDto.UserId,
            ProductId = cartDto.ProductId,
            Quantity = cartDto.Quantity
        };
        var updatedItem = await cartRepo.UpdateCart(cart);
        return cartDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await cartRepo.DeleteAsync(id);
    }
}