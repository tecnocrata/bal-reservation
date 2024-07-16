using Reservations.Core.ValueObjects;
using Shared.Core.Results;

namespace Reservations.Core;

public interface IReservationRepository
{
  Task MakeAsync(Reservation reservation);
  Task<Reservation?> GetByIdAsync(ReservationId id);
  Task<Result> UpdateAsync(Reservation reservation);
  Task<Result<IEnumerable<Reservation>>> ListAll();
}