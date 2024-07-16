using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Shared.Core.Bus.Query;
using Shared.Core.Results;
using SystemUsers.UseCases.Login;
using SytemUsers.UseCases.Login;

namespace Api.Endpoints;
public static class AuthorizationEndpointsExtension
{
  public static void MapAuthorizationEndpoints(this WebApplication app)
  {
    app.MapPost("/authorization/token", async ([FromBody] LoginRequest request, IQueryBus queryBus) =>
    {
      var result = await queryBus.Ask<Result<LoginResponse>>(new LoginQuery { UserName = request.UserName, Password = request.Password });
      if (result.IsFailure)
      {
        return Results.BadRequest(result.Error);
      }
      return Results.Ok(result.Value);
    }).WithName("Authorization").WithOpenApi();
  }
}