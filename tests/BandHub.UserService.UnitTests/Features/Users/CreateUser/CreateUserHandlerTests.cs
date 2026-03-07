using BandHub.UserService.Features.Users.CreateUser;
using BandHub.UserService.Features.Users.Domain;
using FluentAssertions;
using Moq;

namespace BandHub.UserService.UnitTests.Features.Users.CreateUser;

public class CreateUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new CreateUserHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateUser_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "john@example.com", "password123");
        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Email.Should().Be(request.Email);
        response.Id.Should().NotBeEmpty();
        response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.Is<User>(u => u.Name == request.Name && u.Email == request.Email), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var request = new CreateUserRequest("", "john@example.com", "password123");

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Name is required*");

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenEmailIsEmpty()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "", "password123");

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Email is required*");

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenPasswordIsEmpty()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "john@example.com", "");

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Password is required*");

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenPasswordIsTooShort()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "john@example.com", "12345");

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Password must have at least 6 characters*");

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenEmailAlreadyExists()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "john@example.com", "password123");
        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Email already exists.");

        _userRepositoryMock.Verify(
            x => x.EmailExistsAsync(request.Email, It.IsAny<CancellationToken>()),
            Times.Once);

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryWithCorrectData_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateUserRequest("Jane Smith", "jane@example.com", "securepass");
        User? capturedUser = null;

        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((user, _) => capturedUser = user)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        capturedUser.Should().NotBeNull();
        capturedUser!.Name.Should().Be(request.Name);
        capturedUser.Email.Should().Be(request.Email);
        capturedUser.PasswordHash.Should().Be(request.Password);
        capturedUser.Id.Should().NotBeEmpty();
        capturedUser.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
