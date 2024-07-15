using Shared.Entities.ValueObjects;

namespace Reservations.Core.ValueObjects;

public class ReservationId : UuidValueObject
{
  public ReservationId(string value) : base(value) { }
}