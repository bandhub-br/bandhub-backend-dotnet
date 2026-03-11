namespace BandHub.UserService.Features.Accounts.GetAccounts;

public sealed record GetAccountsResponse(Guid Id, string Name, string Email, DateTime CreatedAt);
