using Reservations.Core;
using Reservations.Core.ValueObjects;
using Shared.Core.Bus.Query;
using Shared.Core.Results;

namespace Reservations.UseCases;

public class DetailQueryHandler : IQueryHandler<DetailQuery, Result<DetailResponse>>
{
  private readonly IReservationRepository _reservationRepository;

  public DetailQueryHandler(IReservationRepository reservationRepository)
  {
    _reservationRepository = reservationRepository;
  }

  public async Task<Result<DetailResponse>> Handle(DetailQuery query)
  {
    var reservation = await _reservationRepository.GetByIdAsync(new ReservationId(query.ReservationId));
    if (reservation == null)
    {
      return Result.Failure<DetailResponse>(new Error("reservation-not-found", "Reservation not found"));
    }

    return Result<DetailResponse>.Success(new DetailResponse
    {
      Id = reservation.Id.ValueUuid,
      Status = reservation.Status.ToString(),
      Date = reservation.Date.Value,
      Name = reservation.Name.Value
    });
  }

}