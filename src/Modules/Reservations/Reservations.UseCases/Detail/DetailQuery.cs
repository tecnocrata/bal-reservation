using Shared.Core.Bus.Query;

namespace Reservations.UseCases;

public class DetailQuery : IQuery
{
  public Guid ReservationId { get; set; }
}