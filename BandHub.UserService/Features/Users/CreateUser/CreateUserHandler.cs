using BandHub.UserService.Features.Users.Domain;

namespace BandHub.UserService.Features.Users.CreateUser;

public class CreateUserHandler
{
    private readonly IUserRepository _userRepository;

    public CreateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CreateUserResponse> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateUserValidator();
        var errors = validator.Validate(request);

        if (errors.Count != 0)
            throw new ArgumentException(string.Join(" ", errors));

        var emailExists = await _userRepository.EmailExistsAsync(request.Email, cancellationToken);

        if (emailExists)
            throw new InvalidOperationException("Email already exists.");

        // Temporário: depois trocamos por hash real
        var user = new User(request.Name, request.Email, request.Password);

        await _userRepository.AddAsync(user, cancellationToken);

        return new CreateUserResponse(user.Id, user.Name, user.Email, user.CreatedAt);
    }
}