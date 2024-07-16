using Reservations.Core;
using Reservations.Core.ValueObjects;
using Shared.Core.Bus.Command;
using Shared.Core.Results;

namespace Reservations.UseCases;

public class MakeCommandHandler : ICommandHandler<MakeCommand, Result>
{
  private readonly IReservationRepository _reservationRepository;

  public MakeCommandHandler(IReservationRepository reservationRepository)
  {
    _reservationRepository = reservationRepository;
  }

  public async Task<Result> Handle(MakeCommand command)
  {
    var reservation = new Reservation(new ReservationId(command.Id), new ReservationStatus(ReservationStatusEnum.Open), new ReservationDate(command.Date), new ReservationOwnerName(command.Name), new NumberOfGuests(command.Guests));
    if (reservation.Date.Value < DateTime.UtcNow)
    {
      return Result.Failure(new Error("reservation-date-in-the-past", "Reservation date must be in the future."));
    }
    if (reservation.Date.Value > DateTime.UtcNow.AddDays(30))
    {
      return Result.Failure(new Error("reservation-date-too-far", "Reservation date must be within 30 days."));
    }
    if (reservation.Date.Value.TimeOfDay < new TimeSpan(19, 0, 0) || reservation.Date.Value.TimeOfDay > new TimeSpan(22, 0, 0))
    {
      return Result.Failure(new Error("reservation-time-outside-range", "Reservation time must be between 7:00 PM and 10:00 PM."));
    }

    await _reservationRepository.MakeAsync(reservation);
    return Result.Success();
  }

}