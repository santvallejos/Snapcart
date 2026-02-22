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
    public class BuyRepository : IBuyRepository
    {
        private readonly SnapcartDbContext _db;

        public BuyRepository(SnapcartDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Buy>> GetBuysByUserIdAsync(Guid Id)
        {
            return await _db.Buys.Where(b => b.UserId == Id).ToListAsync();
        }

        public async Task<Buy?> GetBuyByIdAsync(Guid Id)
        {
            return await _db.Buys.FindAsync(Id);
        }

        public async Task<Buy> AddBuyAsync(Buy buy)
        {
            if (buy == null)
                throw new ArgumentNullException(nameof(buy));

            try
            {
                var entry = await _db.Buys.AddAsync(buy);
                await _db.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

        public async Task<Buy> UpdateBuyAsync(Buy buy)
        {
            if (buy == null)
                throw new ArgumentNullException(nameof(buy));

            try
            {
                var entry = _db.Buys.Update(buy);
                await _db.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

        public async Task DeleteBuyAsync(Guid Id)
        {
            var buy = GetBuyByIdAsync(Id).Result;

            if (buy == null)
                throw new ArgumentNullException(nameof(buy));

            try
            {
                _db.Buys.Remove(buy);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }
    }
}
