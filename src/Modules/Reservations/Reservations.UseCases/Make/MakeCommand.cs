using Reservations.Core.ValueObjects;
using Shared.Core.Bus.Command;

namespace Reservations.UseCases;

public class MakeCommand : ICommand
{
  public string Id { get; set; }
  public DateTime Date { get; set; }
  public int Guests { get; set; }
  public string Name { get; set; }

  public MakeCommand(string id, DateTime date, int guests, string name)
  {
    Id = id;
    Date = date;
    Guests = guests;
    Name = name;
  }
}






