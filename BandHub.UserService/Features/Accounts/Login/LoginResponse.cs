namespace BandHub.UserService.Features.Accounts.Login
{
    public sealed record LoginResponse(Guid AccountId, string Name, string Email, string AccountType);
}
