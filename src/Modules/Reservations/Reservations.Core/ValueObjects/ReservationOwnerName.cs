using Shared.Entities.ValueObjects;

namespace Reservations.Core.ValueObjects;

public class ReservationOwnerName : StringValueObject
{
  public ReservationOwnerName(string value) : base(value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      throw new ArgumentException("Owner name cannot be empty.", nameof(value));
    }

    if (value.Length > 100)
    {
      throw new ArgumentException("Owner name must be less than or equal to 100 characters.", nameof(value));
    }
  }
}