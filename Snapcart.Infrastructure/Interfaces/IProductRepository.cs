using System;
using System.Collections.Generic;
using System.Text;
using Snapcart.Domain.Entities;

namespace Snapcart.Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetProductsByListIdAsync(Guid ListId);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
    }
}
