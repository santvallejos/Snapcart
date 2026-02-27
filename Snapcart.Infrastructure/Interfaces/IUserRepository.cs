using Snapcart.Domain.Entities;

namespace Snapcart.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid Id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid Id);
}
