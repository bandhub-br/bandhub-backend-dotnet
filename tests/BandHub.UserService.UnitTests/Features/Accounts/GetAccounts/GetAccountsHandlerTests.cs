using BandHub.UserService.Features.Accounts.Domain;
using BandHub.UserService.Features.Accounts.GetAccounts;
using FluentAssertions;
using Moq;

namespace BandHub.UserService.UnitTests.Features.Accounts.GetAccounts;

public class GetAccountsHandlerTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly GetAccountsHandler _handler;

    public GetAccountsHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _handler = new GetAccountsHandler(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAccount_WhenEmailExists()
    {
        // Arrange
        var account = new Account("John Doe", "john@example.com", "hash1", AccountType.User);

        _accountRepositoryMock
            .Setup(x => x.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        // Act
        var result = await _handler.HandleAsync("john@example.com", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("john@example.com");
        result.Id.Should().Be(account.Id);
        result.CreatedAt.Should().Be(account.CreatedAt);

        _accountRepositoryMock.Verify(
            x => x.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnCorrectAccountType_WhenAccountExists()
    {
        // Arrange
        var account = new Account("Band Name", "band@example.com", "hash", AccountType.Band);

        _accountRepositoryMock
            .Setup(x => x.GetByEmailAsync("band@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        // Act
        var result = await _handler.HandleAsync("band@example.com", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccountType.Should().Be(AccountType.Band);
    }

    [Fact]
    public async Task HandleAsync_ShouldMapAllProperties_WhenAccountExists()
    {
        // Arrange
        var account = new Account("Test Account", "test@example.com", "hash", AccountType.User);

        _accountRepositoryMock
            .Setup(x => x.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        // Act
        var result = await _handler.HandleAsync("test@example.com", CancellationToken.None);

        // Assert
        result.Id.Should().Be(account.Id);
        result.Name.Should().Be(account.Name);
        result.Email.Should().Be(account.Email);
        result.AccountType.Should().Be(account.AccountType);
        result.CreatedAt.Should().Be(account.CreatedAt);
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRepositoryOnce_WhenCalled()
    {
        // Arrange
        var account = new Account("Test", "test@example.com", "hash", AccountType.User);

        _accountRepositoryMock
            .Setup(x => x.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        // Act
        await _handler.HandleAsync("test@example.com", CancellationToken.None);

        // Assert
        _accountRepositoryMock.Verify(
            x => x.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>()),
            Times.Once);

        _accountRepositoryMock.VerifyNoOtherCalls();
    }
}
