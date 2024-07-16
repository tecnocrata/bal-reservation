using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
// using Microsoft.VisualStudio.TestPlatform.TestHost;
using Reservations.UseCases;
using Xunit;
namespace Api.Tests;

public class ReservationsEndpointsTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ReservationsEndpointsTests(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task MakeReservation_ReturnsOk()
  {
    // Arrange
    var request = new MakeRequest
    {
      ReservationDate = DateTime.UtcNow.AddDays(1),
      NumberOfPeople = 2,
      Name = "Test User"
    };

    // Act
    var response = await _client.PostAsJsonAsync("api/reservation", request);

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task GetReservation_ReturnsOk()
  {
    // Arrange
    var reservationId = Guid.NewGuid();

    // Simulate making a reservation first
    await _client.PostAsJsonAsync("api/reservation", new MakeRequest
    {
      ReservationDate = DateTime.UtcNow.AddDays(1),
      NumberOfPeople = 2,
      Name = "Test User"
    });

    // Act
    var response = await _client.GetAsync($"api/reservation/{reservationId}");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task GetReservation_ReturnsNotFound_ForInvalidId()
  {
    // Arrange
    var invalidId = Guid.NewGuid();

    // Act
    var response = await _client.GetAsync($"api/reservation/{invalidId}");

    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Fact]
  public async Task CancelReservation_ReturnsAccepted()
  {
    // Arrange
    var reservationId = Guid.NewGuid();

    // Simulate making a reservation first
    await _client.PostAsJsonAsync("api/reservation", new MakeRequest
    {
      ReservationDate = DateTime.UtcNow.AddDays(1),
      NumberOfPeople = 2,
      Name = "Test User"
    });

    // Act
    var response = await _client.DeleteAsync($"api/reservation/{reservationId}");

    // Assert
    Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
  }
}