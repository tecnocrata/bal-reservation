using Microsoft.AspNetCore.OpenApi;

namespace Api.Endpoints;
public static class AuthorizationEndpointsExtension
{
  public static void MapAuthorizationEndpoints(this WebApplication app)
  {
    app.MapPost("/authorization/token", (string userName, string password) => { return Results.Ok(); }).WithName("Authorization").WithOpenApi();
  }
}