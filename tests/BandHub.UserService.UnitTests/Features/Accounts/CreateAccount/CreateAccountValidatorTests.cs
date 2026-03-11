using BandHub.UserService.Features.Accounts.CreateAccount;
using BandHub.UserService.Features.Accounts.Domain;
using FluentAssertions;

namespace BandHub.UserService.UnitTests.Features.Accounts.CreateAccount;

public class CreateAccountValidatorTests
{
    private readonly RegisterAccountValidator _validator;

    public CreateAccountValidatorTests()
    {
        _validator = new RegisterAccountValidator();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenNameIsEmpty()
    {
        // Arrange
        var request = new RegisterAccountRequest("", "test@example.com", "password123", AccountType.User);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenNameIsWhitespace()
    {
        // Arrange
        var request = new RegisterAccountRequest("   ", "test@example.com", "password123", AccountType.User);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenEmailIsEmpty()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "", "password123", AccountType.User);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Email is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenEmailIsWhitespace()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "   ", "password123", AccountType.User);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Email is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenPasswordIsEmpty()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "test@example.com", "", AccountType.User);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Password is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenPasswordIsWhitespace()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "test@example.com", "   ", AccountType.User);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Password is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenPasswordIsTooShort()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "test@example.com", "12345", AccountType.User);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Password must have at least 6 characters.");
    }

    [Fact]
    public void Validate_ShouldReturnMultipleErrors_WhenMultipleFieldsAreInvalid()
    {
        // Arrange
        var request = new RegisterAccountRequest("", "", "123", AccountType.User);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().HaveCount(3);
        errors.Should().Contain("Name is required.");
        errors.Should().Contain("Email is required.");
        errors.Should().Contain("Password must have at least 6 characters.");
    }

    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "test@example.com", "password123", AccountType.User);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenPasswordIsExactly6Characters()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "test@example.com", "123456", AccountType.User);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().BeEmpty();
    }
}
