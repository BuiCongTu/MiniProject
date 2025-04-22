using Server.Models;

namespace Server.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task ActiveAsync(int id);
}