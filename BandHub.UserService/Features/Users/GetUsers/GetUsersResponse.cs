namespace BandHub.UserService.Features.Users.GetUsers;

public sealed record GetUsersResponse(Guid Id, string Name, string Email, DateTime CreatedAt);