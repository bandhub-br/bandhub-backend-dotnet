using BandHub.UserService.Features.Users.Domain;
using BandHub.UserService.Features.Users.GetUsers;
using FluentAssertions;
using Moq;

namespace BandHub.UserService.UnitTests.Features.Users.GetUsers;

public class GetUsersHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUsersHandler _handler;

    public GetUsersHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetUsersHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAllUsers_WhenUsersExist()
    {
        // Arrange
        var users = new List<User>
        {
            new("John Doe", "john@example.com", "hash1"),
            new("Jane Smith", "jane@example.com", "hash2"),
            new("Bob Johnson", "bob@example.com", "hash3")
        };

        _userRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("John Doe");
        result[0].Email.Should().Be("john@example.com");
        result[1].Name.Should().Be("Jane Smith");
        result[1].Email.Should().Be("jane@example.com");
        result[2].Name.Should().Be("Bob Johnson");
        result[2].Email.Should().Be("bob@example.com");

        _userRepositoryMock.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();

        _userRepositoryMock.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnCorrectResponseFormat_WhenUsersExist()
    {
        // Arrange
        var user = new User("Test User", "test@example.com", "hash");
        _userRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { user });

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        var response = result[0];
        response.Id.Should().Be(user.Id);
        response.Name.Should().Be(user.Name);
        response.Email.Should().Be(user.Email);
        response.CreatedAt.Should().Be(user.CreatedAt);
    }

    [Fact]
    public async Task HandleAsync_ShouldMapAllUserProperties_WhenMultipleUsersExist()
    {
        // Arrange
        var user1 = new User("User One", "user1@test.com", "hash1");
        var user2 = new User("User Two", "user2@test.com", "hash2");

        _userRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { user1, user2 });

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(user1.Id);
        result[0].Name.Should().Be(user1.Name);
        result[0].Email.Should().Be(user1.Email);
        result[0].CreatedAt.Should().Be(user1.CreatedAt);

        result[1].Id.Should().Be(user2.Id);
        result[1].Name.Should().Be(user2.Name);
        result[1].Email.Should().Be(user2.Email);
        result[1].CreatedAt.Should().Be(user2.CreatedAt);
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryOnce_WhenCalled()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());

        // Act
        await _handler.HandleAsync(CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        _userRepositoryMock.VerifyNoOtherCalls();
    }
}
