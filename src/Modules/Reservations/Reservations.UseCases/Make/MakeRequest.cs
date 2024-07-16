namespace Reservations.UseCases;

public class MakeRequest
{
  public string? Name { get; set; }
  public DateTime ReservationDate { get; set; }
  public int NumberOfPeople { get; set; }
}