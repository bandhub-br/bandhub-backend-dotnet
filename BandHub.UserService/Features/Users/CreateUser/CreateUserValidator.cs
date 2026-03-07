namespace BandHub.UserService.Features.Users.CreateUser;

public class CreateUserValidator
{
    public List<string> Validate(CreateUserRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add("Name is required.");

        if (string.IsNullOrWhiteSpace(request.Email))
            errors.Add("Email is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            errors.Add("Password is required.");

        if (!string.IsNullOrWhiteSpace(request.Password) && request.Password.Length < 6)
            errors.Add("Password must have at least 6 characters.");

        return errors;
    }
}