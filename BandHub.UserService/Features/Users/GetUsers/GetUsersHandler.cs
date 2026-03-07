using BandHub.UserService.Features.Users.Domain;

namespace BandHub.UserService.Features.Users.GetUsers;

public class GetUsersHandler
{
    private readonly IUserRepository _userRepository;

    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<GetUsersResponse>> HandleAsync(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        return users
            .Select(user => new GetUsersResponse(user.Id, user.Name, user.Email, user.CreatedAt))
            .ToList();
    }
}