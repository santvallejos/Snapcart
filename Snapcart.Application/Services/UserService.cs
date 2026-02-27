using System.Security.Cryptography;
using Snapcart.Application.Dtos;
using Snapcart.Application.Interfaces;
using Snapcart.Domain.Entities;
using Snapcart.Infrastructure.Interfaces;

namespace Snapcart.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<User> CreateUserAsync(UserDto userDto)
    {
        var existingUser = await userRepository.GetUserByEmailAsync(userDto.Email);
        if (existingUser is not null)
            throw new InvalidOperationException("A user with this email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = userDto.Name,
            LastName = userDto.LastName,
            Email = userDto.Email,
            HashPassword = HashPassword(userDto.Password),
            Phone = userDto.Phone,
            CreatedAt = DateTime.UtcNow
        };

        return await userRepository.AddUserAsync(user);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await userRepository.GetAllUsersAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await userRepository.GetUserByIdAsync(id);
    }

    private static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
}
