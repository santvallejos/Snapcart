using Microsoft.AspNetCore.Mvc;
using Snapcart.Application.Dtos;
using Snapcart.Application.Interfaces;

namespace Snapcart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
    {
        try
        {
            var user = await userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new
            {
                user.Id,
                user.Name,
                user.LastName,
                user.Email,
                user.Phone,
                user.CreatedAt
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users.Select(u => new
        {
            u.Id,
            u.Name,
            u.LastName,
            u.Email,
            u.Phone,
            u.CreatedAt
        }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user is null)
            return NotFound(new { Error = "User not found." });

        return Ok(new
        {
            user.Id,
            user.Name,
            user.LastName,
            user.Email,
            user.Phone,
            user.CreatedAt
        });
    }
}
