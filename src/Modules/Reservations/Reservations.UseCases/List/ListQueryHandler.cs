using Reservations.Core;
using Reservations.Core.ValueObjects;
using Shared.Core.Bus.Query;
using Shared.Core.Results;
using System.Linq;

namespace Reservations.UseCases;

public class ListQueryHandler : IQueryHandler<ListQuery, Result<ListResponse>>
{
  private readonly IReservationRepository _reservationRepository;

  public ListQueryHandler(IReservationRepository reservationRepository)
  {
    _reservationRepository = reservationRepository;
  }

  public async Task<Result<ListResponse>> Handle(ListQuery query)
  {
    var reservations = await _reservationRepository.ListAll();

    var response = new ListResponse
    {
      Reservations = reservations.Value.Select(reservation => new DetailResponse
      {
        Id = reservation.Id.ValueUuid,
        Status = reservation.Status.ToString(),
        Date = reservation.Date.Value,
        Name = reservation.Name.Value
      }).ToList()
    };

    return Result<ListResponse>.Success(response);
  }
}