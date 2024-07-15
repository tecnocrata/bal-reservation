using Shared.Entities.ValueObjects;

namespace Reservations.Core.ValueObjects;

#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
public enum ReservationStatusEnum
#pragma warning restore S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
{
  Open,
  Closed,
  Cancelled
}
public class ReservationStatus : EnumValueObject<ReservationStatusEnum>
{
  public ReservationStatus(ReservationStatusEnum value) : base(value) { }

  public ReservationStatus(string value) : base(value)
  {
  }
}