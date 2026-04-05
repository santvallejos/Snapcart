using Snapcart.Application.Dtos;
using Snapcart.Domain.Entities;

namespace Snapcart.Application.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(UserDto userDto);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid id);
}
