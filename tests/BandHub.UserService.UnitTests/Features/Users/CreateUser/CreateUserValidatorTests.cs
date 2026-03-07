using BandHub.UserService.Features.Users.CreateUser;
using FluentAssertions;

namespace BandHub.UserService.UnitTests.Features.Users.CreateUser;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator;

    public CreateUserValidatorTests()
    {
        _validator = new CreateUserValidator();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenNameIsEmpty()
    {
        // Arrange
        var request = new CreateUserRequest("", "test@example.com", "password123");

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenNameIsWhitespace()
    {
        // Arrange
        var request = new CreateUserRequest("   ", "test@example.com", "password123");

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenEmailIsEmpty()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "", "password123");

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Email is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenEmailIsWhitespace()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "   ", "password123");

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Email is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenPasswordIsEmpty()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "test@example.com", "");

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Password is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenPasswordIsWhitespace()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "test@example.com", "   ");

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Password is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenPasswordIsTooShort()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "test@example.com", "12345");

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Password must have at least 6 characters.");
    }

    [Fact]
    public void Validate_ShouldReturnMultipleErrors_WhenMultipleFieldsAreInvalid()
    {
        // Arrange
        var request = new CreateUserRequest("", "", "123");

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
        var request = new CreateUserRequest("John Doe", "test@example.com", "password123");

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenPasswordIsExactly6Characters()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "test@example.com", "123456");

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().BeEmpty();
    }
}
