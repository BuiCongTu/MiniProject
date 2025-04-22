using Server.DTOs;
using Server.Models;
using Server.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive
            }).ToList();
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive
            };
        }

        public async Task<ProductDto> AddAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageUrl = productDto.ImageUrl,
                IsActive = true
            };
            await _productRepository.AddAsync(product);
            productDto.Id = product.Id;
            return productDto;
        }

        public async Task UpdateAsync(ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(productDto.Id);
            if (product != null)
            {
                product.Name = productDto.Name;
                product.Price = productDto.Price;
                product.Description = productDto.Description;
                product.ImageUrl = productDto.ImageUrl;
                product.IsActive = productDto.IsActive;
                await _productRepository.UpdateAsync(product);
            }
        }

        public async Task ActiveAsync(int id)
        {
            await _productRepository.ActiveAsync(id);
        }
    }
}