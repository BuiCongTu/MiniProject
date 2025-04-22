using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Repositories;

public class CartRepository : ICartRepository
{
    private readonly DatabaseDbContext db;

    public CartRepository(DatabaseDbContext db)
    {
        this.db = db;
    }

    public async Task<IEnumerable<Cart>> GetCartByUser(string userId)
    {
        return db.Carts
            .Include(c => c.Product)
            .Where(c => c.UserId.Equals(userId))
            .ToList();
    }
    
    //add to cart
    public async Task<Cart> AddToCart(Cart cart)
    {
        var existingCart = await db.Carts
            .SingleOrDefaultAsync(c => c.UserId.Equals(cart.UserId) && c.ProductId.Equals(cart.ProductId));
        if (existingCart is not null)
        {
            existingCart.Quantity += cart.Quantity;
            db.Carts.Update(existingCart);
        }
        else
        {
            db.Carts.Add(cart);
        }
        db.SaveChanges();
        return cart;
    }

    //payment
    public async Task Payment(string userId)
    {
        var cartItem = await db.Carts
            .Include(c => c.Product)
            .Where(c => c.UserId.Equals(userId))
            .ToListAsync();
        if (cartItem is null || cartItem.Any())
        {
            throw new Exception("CartDTO is empty");
        }
        
        var totalPrice = cartItem.Sum(item => item.Quantity * item.Product?.Price ?? 0);
        var account = await db.Users.SingleOrDefaultAsync(u=>u.UserId.Equals(userId));

        if (account is null)
        {
            throw new Exception("User Logout");
        }
        
        account.TotalAmount += totalPrice;
        db.Carts.RemoveRange(cartItem);
        await db.SaveChangesAsync();
    }

    public async Task<Cart> UpdateCart(Cart cart)
    {
        var existingCart = await db.Carts.FindAsync(cart.Id);
        if (existingCart is null)
        {
            throw new Exception("CartDTO not found");
        }
        existingCart.Quantity = cart.Quantity;
        db.Carts.Update(existingCart);
        await db.SaveChangesAsync();
        return existingCart;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cart = await db.Carts.FindAsync(id);
        if (cart is null)
        {
            return false;
        }
        db.Carts.Remove(cart);
        await db.SaveChangesAsync();
        return true;
    }
}