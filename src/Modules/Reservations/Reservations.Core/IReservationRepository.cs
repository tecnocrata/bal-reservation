using Reservations.Core.ValueObjects;

namespace Reservations.Core;

public interface IReservationRepository
{
  Task MakeAsync(Reservation reservation);
  Task<Reservation?> GetByIdAsync(ReservationId id);
  Task CancelAsync(Reservation reservation);
}