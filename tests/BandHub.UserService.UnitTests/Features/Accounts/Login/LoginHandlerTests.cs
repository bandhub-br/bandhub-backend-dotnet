using BandHub.UserService.Features.Accounts.Domain;
using BandHub.UserService.Features.Accounts.Login;
using FluentAssertions;
using Moq;

namespace BandHub.UserService.UnitTests.Features.Accounts.Login;

public class LoginHandlerTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _handler = new LoginHandler(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnLoginResponse_WhenCredentialsAreValid()
    {
        var request = new LoginRequest("john@example.com", "password123");
        var account = new Account("John", "john@example.com", "password123", AccountType.User);

        _accountRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        var response = await _handler.HandleAsync(request, CancellationToken.None);

        response.AccountId.Should().Be(account.Id);
        response.Email.Should().Be(account.Email);
        response.Name.Should().Be(account.Name);
        response.AccountType.Should().Be(AccountType.User.ToString());
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenAccountDoesNotExist()
    {
        var request = new LoginRequest("missing@example.com", "password123");

        _accountRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Account?)null);

        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Credencias Inválidas.");
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowInvalidOperationException_WhenPasswordIsInvalid()
    {
        var request = new LoginRequest("john@example.com", "wrong-password");
        var account = new Account("John", "john@example.com", "password123", AccountType.User);

        _accountRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        var act = async () => await _handler.HandleAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Credencias Inválidas.");
    }
}
