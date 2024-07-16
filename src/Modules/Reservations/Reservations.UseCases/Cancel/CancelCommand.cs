using Shared.Core.Bus.Command;

namespace Reservations.UseCases;

public class CancelCommand : ICommand
{
  public Guid ReservationId { get; set; }
}