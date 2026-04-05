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
    public class ListRepository : IListRepository
    {
        private readonly SnapcartDbContext _db;

        public ListRepository(SnapcartDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<List>> GetAllListsByUserIdAsync(Guid userId)
        {
            return await _db.Lists.Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<List?> GetActiveListByUserIdAsync(Guid userId)
        {
            return await _db.Lists
                .Include(l => l.Products)
                .FirstOrDefaultAsync(l => l.UserId == userId && l.IsActive);
        }

        public async Task<List?> GetListByIdAsync(Guid Id)
        {
            return await _db.Lists.FindAsync(Id);
        }

        public async Task<List> AddListAsync(List list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            try
            {
                var entry = await _db.Lists.AddAsync(list);
                await _db.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

        public async Task<List> UpdateListAsync(List list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            try
            {
                var entry = _db.Lists.Update(list);
                await _db.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

        public async Task DeleteListAsync(Guid id)
        {
            var list = GetListByIdAsync(id).Result;

            if (list == null)
                throw new ArgumentNullException(nameof(list));

            try
            {
                _db.Lists.Remove(list);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }
    }
}
