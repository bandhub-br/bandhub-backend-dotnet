using BandHub.UserService.Features.Accounts.Domain;
using Microsoft.EntityFrameworkCore;

namespace BandHub.UserService.Infrastructure.Persistence;

public class AccountRepository : IAccountRepository
{
    private readonly AccountDbContext _context;

    public AccountRepository(AccountDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken)
    {
        await _context.Accounts.AddAsync(account, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Accounts
            .AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<Account> GetByEmailAsync(string Email, CancellationToken cancellationToken)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(x => x.Email == Email, cancellationToken);
    }
}