namespace BandHub.UserService.Features.Accounts.CreateAccount;

public sealed record RegisterAccountResponse(Guid Id, string Name, string Email, string AccountType, DateTime CreatedAt);
