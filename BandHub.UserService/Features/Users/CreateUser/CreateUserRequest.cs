namespace BandHub.UserService.Features.Users.CreateUser;

public sealed record CreateUserRequest(string Name, string Email, string Password);