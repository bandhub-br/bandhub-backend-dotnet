using BandHub.UserService.Features.Accounts.Domain;

namespace BandHub.UserService.Features.Accounts.CreateAccount;

public class RegisterAccountHandler
{
    private readonly IAccountRepository _accountRepository;

    public RegisterAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<RegisterAccountResponse> HandleAsync(RegisterAccountRequest request, CancellationToken cancellationToken)
    {
        var validator = new RegisterAccountValidator();
        var errors = validator.Validate(request);

        if (errors.Count != 0)
            throw new ArgumentException(string.Join(" ", errors));

        var emailExists = await _accountRepository.EmailExistsAsync(request.Email, cancellationToken);

        if (emailExists)
            throw new InvalidOperationException("Email already exists.");

        // Temporário: depois trocamos por hash real
        var account = new Account(request.Name, request.Email, request.Password, request.AccountType);

        await _accountRepository.AddAsync(account, cancellationToken);

        return new RegisterAccountResponse(
            account.Id, 
            account.Name, 
            account.Email, 
            account.AccountType.ToString(), 
            account.CreatedAt);
    }
}
