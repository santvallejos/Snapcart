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
    public class UserRepository : IUserRepository
    {
        private readonly SnapcartDbContext _db;

        public UserRepository(SnapcartDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid Id)
        {
            return await _db.Users.FindAsync(Id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                var entry = await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                var entry = _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }

        public async Task DeleteUserAsync(Guid Id)
        {
            var user = GetUserByIdAsync(Id).Result;

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: ", ex);
            }
        }
    }
}
