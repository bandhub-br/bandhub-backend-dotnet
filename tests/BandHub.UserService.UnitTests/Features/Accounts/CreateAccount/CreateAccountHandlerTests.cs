using BandHub.UserService.Features.Accounts.CreateAccount;
using BandHub.UserService.Features.Accounts.Domain;
using FluentAssertions;
using Moq;

namespace BandHub.UserService.UnitTests.Features.Accounts.CreateAccount;

public class CreateAccountHandlerTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly RegisterAccountHandler _handler;

    public CreateAccountHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _handler = new RegisterAccountHandler(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateAccount_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "john@example.com", "password123", AccountType.User);
        _accountRepositoryMock
            .Setup(x => x.EmailExistsAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Email.Should().Be(request.Email);
        response.AccountType.Should().Be(AccountType.User.ToString());
        response.Id.Should().NotBeEmpty();
        response.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        _accountRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Account>(a => a.Name == request.Name && a.Email == request.Email), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var request = new RegisterAccountRequest("", "john@example.com", "password123", AccountType.User);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Name is required*");

        _accountRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenEmailIsEmpty()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "", "password123", AccountType.User);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Email is required*");

        _accountRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenPasswordIsEmpty()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "john@example.com", "", AccountType.User);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Password is required*");

        _accountRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowArgumentException_WhenPasswordIsTooShort()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "john@example.com", "12345", AccountType.User);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Password must have at least 6 characters*");

        _accountRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenEmailAlreadyExists()
    {
        // Arrange
        var request = new RegisterAccountRequest("John Doe", "john@example.com", "password123", AccountType.User);
        _accountRepositoryMock
            .Setup(x => x.EmailExistsAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Email already exists.");

        _accountRepositoryMock.Verify(
            x => x.EmailExistsAsync(request.Email, It.IsAny<CancellationToken>()),
            Times.Once);

        _accountRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryWithCorrectData_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterAccountRequest("Jane Smith", "jane@example.com", "securepass", AccountType.Band);
        Account? capturedAccount = null;

        _accountRepositoryMock
            .Setup(x => x.EmailExistsAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _accountRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
            .Callback<Account, CancellationToken>((account, _) => capturedAccount = account)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        capturedAccount.Should().NotBeNull();
        capturedAccount!.Name.Should().Be(request.Name);
        capturedAccount.Email.Should().Be(request.Email);
        capturedAccount.PasswordHash.Should().Be(request.Password);
        capturedAccount.AccountType.Should().Be(AccountType.Band);
        capturedAccount.Id.Should().NotBeEmpty();
        capturedAccount.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
