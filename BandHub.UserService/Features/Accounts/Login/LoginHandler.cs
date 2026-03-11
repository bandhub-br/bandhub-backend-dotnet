using BandHub.UserService.Features.Accounts.Domain;

namespace BandHub.UserService.Features.Accounts.Login
{
    public class LoginHandler
    {
        private readonly IAccountRepository _accountRepository;
        public LoginHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<LoginResponse> HandleAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (account == null) // Temporário: depois trocamos por hash real
                throw new InvalidOperationException("Credencias Inválidas.");

            if (account.PasswordHash != request.Password) // Temporário: depois trocamos por hash real
                throw new InvalidOperationException("Credencias Inválidas.");

            return new LoginResponse(
                account.Id,
                account.Name,
                account.Email,
                account.AccountType.ToString());
        }
    }
}
