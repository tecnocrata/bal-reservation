using Shared.Entities.ValueObjects;

namespace Reservations.Core.ValueObjects;

public class NumberOfGuests : IntValueObject
{
  public NumberOfGuests(int value) : base(value)
  {
    if (value <= 0)
    {
      throw new ArgumentException("Number of guests must be greater than 0.", nameof(value));
    }

    if (value > 100)
    {
      throw new ArgumentException("Number of guests must be less than or equal to 100.", nameof(value));
    }
  }
}