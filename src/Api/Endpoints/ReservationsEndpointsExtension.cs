using Microsoft.AspNetCore.OpenApi;

namespace Api.Endpoints;
public static class ReservationsEndpointsExtension
{
  public static void MapReservationEndpoints(this WebApplication app)
  {
    app.MapGet("api/reservation/{id}", (int id) => { return Results.Ok(); }).WithName("Reservations").RequireAuthorization().WithOpenApi();
  }
}