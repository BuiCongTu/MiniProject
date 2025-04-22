using Server.DTOs;

namespace Server.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto> GetByIdAsync(int id);
    Task<ProductDto> AddAsync(ProductDto productDto);
    Task UpdateAsync(ProductDto productDto);
    Task ActiveAsync(int id);
}