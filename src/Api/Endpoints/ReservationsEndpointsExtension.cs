using Microsoft.AspNetCore.OpenApi;
using Reservations.UseCases;
using Shared.Core.Bus.Command;
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
    }).WithName("Reservations").RequireAuthorization().WithOpenApi();
  }
}