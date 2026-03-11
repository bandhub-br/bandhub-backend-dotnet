using BandHub.UserService.Features.Accounts.Domain;

namespace BandHub.UserService.Features.Accounts.GetAccounts;

public class GetAccountsHandler
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountsHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Account> HandleAsync(string email, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByEmailAsync(email, cancellationToken);

        return account;
        
    }
}
