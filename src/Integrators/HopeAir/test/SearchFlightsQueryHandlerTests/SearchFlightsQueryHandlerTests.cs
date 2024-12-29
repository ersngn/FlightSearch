using FluentValidation;
using HopeAir.Application.Extensions;
using HopeAir.Application.Features.Flight.Queries.SearchFlights;
using HopeAir.Application.Interfaces;
using HopeAir.Domain.Models;
using Moq;

namespace SearchFlightsQueryHandlerTests;

public class SearchFlightsQueryHandlerTests
{
    private readonly Mock<ISoapClientService> _mockSoapClientService;
    private readonly Mock<IValidator<SearchFlightsQuery>> _mockValidator;
    private readonly SearchFlightsQueryHandler _handler;

    public SearchFlightsQueryHandlerTests()
    {
        _mockSoapClientService = new Mock<ISoapClientService>();
        _mockValidator = new Mock<IValidator<SearchFlightsQuery>>();
        _handler = new SearchFlightsQueryHandler(_mockSoapClientService.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenValidationFails()
    {
        // Arrange
        var query = new SearchFlightsQuery
        (
            "IST",
            "",
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(4),
            2
        );

        var validationResult = new FluentValidation.Results.ValidationResult(new[]
        {
            new FluentValidation.Results.ValidationFailure("Destination", "Destination is required."),
            new FluentValidation.Results.ValidationFailure("PassengerCount",
                "Passenger count must be greater than zero.")
        });

        _mockValidator.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Validation failed.", result.Message);
        Assert.NotEmpty(result.Errors);
    }
}