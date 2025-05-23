using Server.DTOs;

namespace Server.Services;
public interface ICartService
{
    Task<IEnumerable<CartDTO>> GetCartByUser(string userId);
    Task<CartDTO> AddToCart(CartDTO cartDto);
    Task Payment(string userId);
    Task<CartDTO> UpdateCart(CartDTO cartDto);
    Task<bool> DeleteAsync(int id);
}