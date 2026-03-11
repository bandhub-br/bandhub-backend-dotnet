namespace BandHub.UserService.Features.Accounts.CreateAccount;

public class RegisterAccountValidator
{
    public List<string> Validate(RegisterAccountRequest request)
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
