using Snapcart.Application.Dtos;
using Snapcart.Domain.Entities;

namespace Snapcart.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsByUserAsync(Guid userId);
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<Product> AddProductAsync(Guid userId, ProductDto productDto);
    Task<Product> UpdateProductAsync(Guid productId, ProductDto productDto);
    Task DeleteProductAsync(Guid productId);
    Task<Product?> DetectAndMarkProductAsync(Guid userId, Stream imageStream, string fileName);
    Task<Buy> CompletePurchaseAsync(Guid userId, BuyDto buyDto);
}
