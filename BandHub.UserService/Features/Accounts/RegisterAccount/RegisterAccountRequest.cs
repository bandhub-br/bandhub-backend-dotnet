using BandHub.UserService.Features.Accounts.Domain;

namespace BandHub.UserService.Features.Accounts.CreateAccount;

public sealed record RegisterAccountRequest(string Name, string Email, string Password, AccountType AccountType);
