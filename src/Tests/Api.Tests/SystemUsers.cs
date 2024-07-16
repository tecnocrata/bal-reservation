using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Reservations.UseCases;
using SystemUsers.UseCases.Login;
using Xunit;
namespace Api.Tests;


public class AuthorizationEndpointsTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public AuthorizationEndpointsTests(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task TokenGeneration_ReturnsOk_WithValidCredentials()
  {
    // Arrange
    var loginRequest = new LoginRequest
    {
      UserName = "eortuno",
      Password = "123456"
    };

    // Act
    var response = await _client.PostAsJsonAsync("/authorization/token", loginRequest);

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    // Verify that the response contains a token
    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
    Assert.NotNull(loginResponse);
  }

  [Fact]
  public async Task TokenGeneration_ReturnsBadRequest_WithInvalidCredentials()
  {
    // Arrange
    var loginRequest = new LoginRequest
    {
      UserName = "invaliduser",
      Password = "invalidpassword"
    };

    // Act
    var response = await _client.PostAsJsonAsync("/authorization/token", loginRequest);

    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Fact]
  public async Task TokenGeneration_ReturnsBadRequest_WithMissingCredentials()
  {
    // Arrange
    var loginRequest = new LoginRequest
    {
      UserName = "",
      Password = ""
    };

    // Act
    var response = await _client.PostAsJsonAsync("/authorization/token", loginRequest);

    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }
}