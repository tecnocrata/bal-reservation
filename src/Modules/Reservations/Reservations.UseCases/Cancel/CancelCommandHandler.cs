using Reservations.Core;
using Reservations.Core.ValueObjects;
using Shared.Core.Bus.Command;
using Shared.Core.Results;

namespace Reservations.UseCases;

public class CancelCommandHandler : ICommandHandler<CancelCommand, Result>
{
  private readonly IReservationRepository _reservationRepository;

  public CancelCommandHandler(IReservationRepository reservationRepository)
  {
    _reservationRepository = reservationRepository;
  }

  public async Task<Result> Handle(CancelCommand command)
  {
    var reservation = await _reservationRepository.GetByIdAsync(new ReservationId(command.ReservationId));
    if (reservation == null)
    {
      return Result.Failure(new Error("reservation-not-found", "Reservation not found"));
      // throw new EntityNotFoundException(command.ReservationId, nameof(Reservation));
    }

    reservation.Cancel();
    return await _reservationRepository.UpdateAsync(reservation);
  }

}
