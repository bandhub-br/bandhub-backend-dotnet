using BandHub.UserService.Features.Users.CreateUser;
using BandHub.UserService.Features.Users.Domain;
using BandHub.UserService.Features.Users.GetUsers;
using BandHub.UserService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BandHub.UserService.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddUserService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<CreateUserHandler>();
        services.AddScoped<GetUsersHandler>();

        return services;
    }
}