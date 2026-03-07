using BandHub.BandService.Features.Bands.CreateBand;
using FluentAssertions;

namespace BandHub.BandService.UnitTests.Features.Bands.CreateBand;

public class CreateBandValidatorTests
{
    private readonly CreateBandValidator _validator;

    public CreateBandValidatorTests()
    {
        _validator = new CreateBandValidator();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenNameIsEmpty()
    {
        // Arrange
        var request = new CreateBandRequest("", "Descrição válida", "Rock", null);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenNameIsWhitespace()
    {
        // Arrange
        var request = new CreateBandRequest("   ", "Descrição válida", "Rock", null);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenGenreIsEmpty()
    {
        // Arrange
        var request = new CreateBandRequest("Arctic Monkeys", "Descrição válida", "", null);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Genre is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenGenreIsWhitespace()
    {
        // Arrange
        var request = new CreateBandRequest("Arctic Monkeys", "Descrição válida", "   ", null);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Genre is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenDescriptionIsEmpty()
    {
        // Arrange
        var request = new CreateBandRequest("Arctic Monkeys", "", "Indie Rock", null);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Description is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenDescriptionExceeds1000Characters()
    {
        // Arrange
        var longDescription = new string('A', 1001);
        var request = new CreateBandRequest("Arctic Monkeys", longDescription, "Indie Rock", null);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().Contain("Description cannot exceed 1000 characters.");
    }

    [Fact]
    public void Validate_ShouldNotReturnDescriptionLengthError_WhenDescriptionIsExactly1000Characters()
    {
        // Arrange
        var description = new string('A', 1000);
        var request = new CreateBandRequest("Arctic Monkeys", description, "Indie Rock", null);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnMultipleErrors_WhenMultipleFieldsAreInvalid()
    {
        // Arrange
        var request = new CreateBandRequest("", "", "", null);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().HaveCount(3);
        errors.Should().Contain("Name is required.");
        errors.Should().Contain("Genre is required.");
        errors.Should().Contain("Description is required.");
    }

    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateBandRequest("Arctic Monkeys", "Banda inglesa de indie rock", "Indie Rock", null);

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenRequestIsValidWithSpotifyId()
    {
        // Arrange
        var request = new CreateBandRequest("Arctic Monkeys", "Banda inglesa de indie rock", "Indie Rock", "7Ln80lUS6He07XvHI8qqHH");

        // Act
        var errors = _validator.Validate(request);

        // Assert
        errors.Should().BeEmpty();
    }
}
