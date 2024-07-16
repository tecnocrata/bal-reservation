using Reservations.UseCases;
namespace Reservations.UseCases;

public class ListResponse
{
  public List<DetailResponse> Reservations { get; set; } = new List<DetailResponse>();
}