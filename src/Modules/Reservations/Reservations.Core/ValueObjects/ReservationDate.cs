using Shared.Entities.ValueObjects;

namespace Reservations.Core.ValueObjects;

public class ReservationDate : DateTimeValueObject
{
  public ReservationDate(DateTime value) : base(value)
  {

  }
}