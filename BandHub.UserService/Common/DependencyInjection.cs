using BandHub.UserService.Features.Accounts.CreateAccount;
using BandHub.UserService.Features.Accounts.Domain;
using BandHub.UserService.Features.Accounts.GetAccounts;
using BandHub.UserService.Features.Accounts.Login;
using BandHub.UserService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BandHub.UserService.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddUserService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AccountDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAccountRepository, AccountRepository>();

        services.AddScoped<RegisterAccountHandler>();
        services.AddScoped<GetAccountsHandler>();
        services.AddScoped<LoginHandler>();

        return services;
    }
}