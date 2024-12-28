using AybJet.Application.Feature.Flight.Queries.SearchFlights;
using AybJet.Application.Interfaces;
using AybJet.Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace AybJet.UnitTests.QueryTests;

public class SearchFlightsQueryHandlerTests
{
    private readonly Mock<IHttpClientService> _httpClientServiceMock;
    private readonly Mock<IValidator<SearchFlightsQuery>> _validatorMock;
    private readonly SearchFlightsQueryHandler _handler;

    public SearchFlightsQueryHandlerTests()
    {
        _httpClientServiceMock = new Mock<IHttpClientService>();
        _validatorMock = new Mock<IValidator<SearchFlightsQuery>>();
        _handler = new SearchFlightsQueryHandler(_httpClientServiceMock.Object, _validatorMock.Object);
    }
    
    /// <summary>
    /// Handle_ShouldReturnSuccess_WhenFlightsAreFound : Uçuş bulunduğu takdirde 200 dönülmesiin test eder.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenFlightsAreFound()
    {
        // Arrange
        var query = new SearchFlightsQuery("IST", "JFK", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10), 1);
        var mockFlights = new List<Flight>
        {
            new ()
            {
                FlightNumber = "TK001",
                Departure = "IST",
                Arrival = "JFK",
                DepartureTime = DateTime.UtcNow.AddDays(2),
                ArrivalTime = DateTime.UtcNow.AddDays(3),
                Price = 500,
                Currency = "USD",
                Duration = "10h"
            }
        };

        _validatorMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _httpClientServiceMock.Setup(h => h.PostAsync<Domain.Models.Flight>("mock-url", query))
            .ReturnsAsync(mockFlights);

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal("TK001", response.Data.First().FlightNumber);
    }
    
    /// <summary>
    /// Handle_ShouldReturnValidationError_WhenValidationFails : Request içinde bir validation error oluşursa Error response dönülmesini test eder.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenValidationFails()
    {
        // Arrange
        var query = new SearchFlightsQuery("", "JFK", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10), 1);

        var validationFailures = new List<ValidationFailure>
        {
            new ("Origin", "Origin is required."),
            new ("Destination", "Destination is required.")
        };

        _validatorMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failed.", response.Message);
        Assert.Contains("Origin is required.", response.Errors);
        Assert.Contains("Destination is required.", response.Errors);
    }
    
    /// <summary>
    /// Handle_ShouldReturnSuccess_WithEmptyList_WhenNoFlightsFound : Hiç uçuş bulanamaması durumunda yine de api'nin 200 dönmesini test eder.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithEmptyList_WhenNoFlightsFound()
    {
        // Arrange
        var query = new SearchFlightsQuery("IST", "JFK", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10), 1);

        _validatorMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult()); 
        
        _httpClientServiceMock.Setup(h => h.PostAsync<Domain.Models.Flight>("mock-url", query))
            .ReturnsAsync(new List<Domain.Models.Flight>());

        // Act
        var response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Empty(response.Data); // No flights
        Assert.Equal("No flights found.", response.Message);
    }

    /// <summary>
    /// Handle_ShouldProcessLargeNumberOfFlightsWithinTimeLimit : yüksek sayıdaki uçuş verilerini 5 saniye içinde getirebileceğini test eder.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldProcessLargeNumberOfFlightsWithinTimeLimit()
    {
        // Arrange
        var query = new SearchFlightsQuery("IST", "JFK", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10), 1);
        var mockFlights = Enumerable.Range(1, 10000).Select(i => new Domain.Models.Flight
        {
            FlightNumber = $"TK{i:D4}",
            Departure = "IST",
            Arrival = "JFK",
            DepartureTime = DateTime.UtcNow.AddDays(2),
            ArrivalTime = DateTime.UtcNow.AddDays(3),
            Price = 500 + i,
            Currency = "USD",
            Duration = "10h"
        }).ToList();

        _validatorMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _httpClientServiceMock.Setup(h => h.PostAsync<Domain.Models.Flight>("mock-url", query))
            .ReturnsAsync(mockFlights);

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _handler.Handle(query, CancellationToken.None);
        stopwatch.Stop();

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Data);
        Assert.Equal(10000, response.Data.Count());
        Assert.True(stopwatch.ElapsedMilliseconds < 5000, "Processing took too long.");
    }
}
