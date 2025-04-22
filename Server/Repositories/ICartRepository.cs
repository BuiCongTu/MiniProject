using Server.Models;

namespace Server.Repositories;
public interface ICartRepository
{
    Task<IEnumerable<Cart>> GetCartByUser(string userId);
    Task<Cart> AddToCart(Cart cart);
    Task Payment(string userId);
}