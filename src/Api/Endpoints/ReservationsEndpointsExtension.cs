using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Reservations.UseCases;
using Shared.Core.Bus.Command;
using Shared.Core.Bus.Query;
using Shared.Core.Results;

namespace Api.Endpoints;
public static class ReservationsEndpointsExtension
{
  public static void MapReservationEndpoints(this WebApplication app)
  {
    app.MapDelete("api/reservation/{id}", async (Guid id, ICommandBus bus) =>
    {
      var response = await bus.Dispatch<Result>(new CancelCommand { ReservationId = id });
      if (response.IsFailure)
      {
        return Results.BadRequest(response.Error);
      }
      return Results.Accepted();
    }).WithName("CancelReservation").RequireAuthorization().WithOpenApi();

    app.MapGet("api/reservation/{id}", async (Guid id, IQueryBus bus) =>
    {
      var response = await bus.Ask<Result<DetailResponse>>(new DetailQuery { ReservationId = id });
      if (response.IsFailure)
      {
        return Results.BadRequest(response.Error);
      }
      return Results.Ok(response.Value);
    }).WithName("GetReservation").RequireAuthorization().WithOpenApi();

    app.MapGet("api/reservation", async (IQueryBus bus) =>
    {
      var response = await bus.Ask<Result<ListResponse>>(new ListQuery());
      if (response.IsFailure)
      {
        return Results.BadRequest(response.Error);
      }
      return Results.Ok(response.Value);
    }).WithName("AllReservation").RequireAuthorization().WithOpenApi();

    app.MapPost("api/reservation", async ([FromBody] MakeRequest request, ICommandBus bus) =>
    {
      var response = await bus.Dispatch<Result>(new MakeCommand(Guid.NewGuid().ToString(), request.ReservationDate, request.NumberOfPeople, request.Name!));
      if (response.IsFailure)
      {
        return Results.BadRequest(response.Error);
      }
      return Results.Ok();
    }).WithName("MakeReservation").RequireAuthorization().WithOpenApi();
  }
}