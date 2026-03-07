namespace BandHub.UserService.Features.Users.CreateUser;

public sealed record CreateUserResponse(Guid Id, string Name, string Email, DateTime CreatedAt);