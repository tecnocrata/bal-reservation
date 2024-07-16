using Reservations.Core.ValueObjects;

namespace Reservations.Core;

public class Reservation
{
  public ReservationId Id { get; private set; }
  public ReservationStatus Status { get; private set; }
  public ReservationDate Date { get; private set; }

  public ReservationOwnerName Name { get; private set; }
  public NumberOfGuests NumberOfGuests { get; private set; }

  public Reservation(ReservationId id, ReservationStatus status, ReservationDate date, ReservationOwnerName name, NumberOfGuests numberOfGuests)
  {
    Id = id;
    Status = status;
    Date = date;
    Name = name;
    NumberOfGuests = numberOfGuests;
  }

  public void Cancel()
  {
    Status = new ReservationStatus(ReservationStatusEnum.Cancelled);
  }
}