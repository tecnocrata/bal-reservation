using Microsoft.AspNetCore.OpenApi;

namespace Api.Endpoints;
public static class AuthorizationEndpointsExtension
{
  public static void MapAuthorizationEndpoints(this WebApplication app)
  {
    app.MapPost("/authorization/token", (int id) => { return Results.Ok(); }).WithName("Authorization").WithOpenApi();
  }
}