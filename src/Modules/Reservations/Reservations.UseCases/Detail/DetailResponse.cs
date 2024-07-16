using Shared.Core.Results;

namespace Reservations.UseCases;

public class DetailResponse
{
  public Guid Id { get; set; }
  public string? Status { get; set; }
  public DateTime Date { get; set; }
  public string? Name { get; set; }
}