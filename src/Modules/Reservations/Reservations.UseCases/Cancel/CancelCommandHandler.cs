using Reservations.Core;
using Reservations.Core.ValueObjects;
using Shared.Core.Bus.Command;

namespace Reservations.UseCases;

public class CancelCommandHandler : ICommandHandler<CancelCommand>
{
  private readonly IReservationRepository _reservationRepository;

  public CancelCommandHandler(IReservationRepository reservationRepository)
  {
    _reservationRepository = reservationRepository;
  }

  public async Task Handle(CancelCommand command)
  {
    var reservation = await _reservationRepository.GetByIdAsync(new ReservationId(command.ReservationId));
    // if (reservation == null)
    // {
    //   throw new EntityNotFoundException(command.ReservationId, nameof(Reservation));
    // }

    reservation.Cancel();
    await _reservationRepository.CancelAsync(reservation);
  }

}
