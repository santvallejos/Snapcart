using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snapcart.Infrastructure.Data;
using Snapcart.Infrastructure.Interfaces;
using Snapcart.Domain.Entities;

namespace Snapcart.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SnapcartDbContext _db;

        public ProductRepository(SnapcartDbContext db)
        {
            _db = db;
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _db.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsByListIdAsync(Guid listId)
        {
            return await _db.Products.Where(p => p.ListId == listId).ToListAsync();
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            try
            {
                var entry = await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            try
            {
                var entry = _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

        public async Task DeleteProductAsync(Guid Id)
        {
            var product = GetProductByIdAsync(Id).Result;

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            try
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }
    }
}
