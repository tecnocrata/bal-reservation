using Shared.Entities.ValueObjects;

namespace Reservations.Core.ValueObjects;

public class ReservationId : UuidValueObject
{
  public ReservationId(string value) : base(value) { }

  public ReservationId(Guid value) : base(value.ToString()) { }
}