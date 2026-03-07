using BandHub.UserService.Data;
using BandHub.UserService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BandHub.UserService.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserDbContext _context;

    public UsersController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await _context.Users
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();

        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create([FromBody] CreateUserRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = request.Password,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Created($"/users/{user.Id}", user);
    }
}

public record CreateUserRequest(string Name, string Email, string Password);