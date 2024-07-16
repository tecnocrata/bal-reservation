using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Reservations.UseCases;
using SystemUsers.UseCases.Login;
using Xunit;
namespace Api.Tests;

public class ReservationsEndpointsTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ReservationsEndpointsTests(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  private async Task<string> GetAuthTokenAsync()
  {
    var loginRequest = new LoginRequest
    {
      UserName = "enrique.ortuno",
      Password = "123456"
    };

    var response = await _client.PostAsJsonAsync("/authorization/token", loginRequest);
    response.EnsureSuccessStatusCode();

    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
    return loginResponse?.Token!;
  }

  private void AddAuthorizationHeader(string token)
  {
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }

  [Fact]
  public async Task MakeReservation_ReturnsOk()
  {
    // Arrange
    var token = await GetAuthTokenAsync();
    AddAuthorizationHeader(token);

    var reservationDate = DateTime.UtcNow.Date.AddDays(1).AddHours(19); // 7 PM next day

    var request = new MakeRequest
    {
      ReservationDate = reservationDate,
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
    var token = await GetAuthTokenAsync();
    AddAuthorizationHeader(token);

    var reservationId = new Guid("05b9a2bf-073b-4b3b-a28e-d7c9d37ec519");

    // Act
    var response = await _client.GetAsync($"api/reservation/{reservationId}");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    // Verify that the response contains data
    var detailResponse = await response.Content.ReadFromJsonAsync<DetailResponse>();
    Assert.NotNull(detailResponse);
  }

  [Fact]
  public async Task GetReservation_ReturnsNotFound_ForInvalidId()
  {
    // Arrange
    var token = await GetAuthTokenAsync();
    AddAuthorizationHeader(token);

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
    var token = await GetAuthTokenAsync();
    AddAuthorizationHeader(token);

    var reservationId = new Guid("05b9a2bf-073b-4b3b-a28e-d7c9d37ec519");
    // Simulate making a reservation first
    await _client.PostAsJsonAsync("api/reservation", new Guid("05b9a2bf-073b-4b3b-a28e-d7c9d37ec519"));

    // Act
    var response = await _client.DeleteAsync($"api/reservation/{reservationId}");

    // Assert
    Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
  }

  [Fact]
  public async Task Endpoints_ReturnUnauthorized_ForInvalidToken()
  {
    // Arrange
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid_token");

    // Act
    var makeResponse = await _client.PostAsJsonAsync("api/reservation", new MakeRequest());
    var getResponse = await _client.GetAsync("api/reservation/some-id");
    var deleteResponse = await _client.DeleteAsync("api/reservation/some-id");

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, makeResponse.StatusCode);
    Assert.Equal(HttpStatusCode.Unauthorized, getResponse.StatusCode);
    Assert.Equal(HttpStatusCode.Unauthorized, deleteResponse.StatusCode);
  }

  [Fact]
  public async Task Endpoints_ReturnUnauthorized_ForNoToken()
  {
    // Arrange
    _client.DefaultRequestHeaders.Authorization = null;

    // Act
    var makeResponse = await _client.PostAsJsonAsync("api/reservation", new MakeRequest());
    var getResponse = await _client.GetAsync("api/reservation/some-id");
    var deleteResponse = await _client.DeleteAsync("api/reservation/some-id");

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, makeResponse.StatusCode);
    Assert.Equal(HttpStatusCode.Unauthorized, getResponse.StatusCode);
    Assert.Equal(HttpStatusCode.Unauthorized, deleteResponse.StatusCode);
  }
}