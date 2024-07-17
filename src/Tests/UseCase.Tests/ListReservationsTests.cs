using Moq;
using Reservations.Core;
using Reservations.Core.ValueObjects;
using Reservations.UseCases;
using Shared.Core.Results;

namespace UseCase.Tests;

public class ListQueryHandlerTests
{
  private readonly Mock<IReservationRepository> _reservationRepositoryMock;
  private readonly ListQueryHandler _handler;

  public ListQueryHandlerTests()
  {
    _reservationRepositoryMock = new Mock<IReservationRepository>();
    _handler = new ListQueryHandler(_reservationRepositoryMock.Object);
  }

  [Fact]
  public async Task Handle_ReturnsSuccess_WithEmptyList_WhenNoReservationsExist()
  {
    // Arrange
    _reservationRepositoryMock.Setup(r => r.ListAll())
                              .ReturnsAsync(Result<IEnumerable<Reservation>>.Success(Enumerable.Empty<Reservation>()));

    var query = new ListQuery();

    // Act
    var result = await _handler.Handle(query);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Empty(result.Value.Reservations);
  }

  [Fact]
  public async Task Handle_ReturnsSuccess_WithReservations_WhenReservationsExist()
  {
    // Arrange
    var reservations = new List<Reservation>
    {
      new Reservation(
        new ReservationId(Guid.NewGuid()),
        new ReservationStatus(ReservationStatusEnum.Open),
        new ReservationDate(DateTime.UtcNow),
        new ReservationOwnerName("John Doe"),
        new NumberOfGuests(2)
      ),
      new Reservation(
        new ReservationId(Guid.NewGuid()),
        new ReservationStatus(ReservationStatusEnum.Closed),
        new ReservationDate(DateTime.UtcNow),
        new ReservationOwnerName("Jane Doe"),
        new NumberOfGuests(4)
      )
    };

    _reservationRepositoryMock.Setup(r => r.ListAll())
                              .ReturnsAsync(Result<IEnumerable<Reservation>>.Success(reservations.AsEnumerable()));

    var query = new ListQuery();

    // Act
    var handleResult = await _handler.Handle(query);

    // Assert
    Assert.True(handleResult.IsSuccess);
    Assert.Equal(2, handleResult.Value.Reservations.Count);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenRepositoryReturnsFailure()
  {
    // Arrange
    _reservationRepositoryMock.Setup(r => r.ListAll())
                              .ReturnsAsync(Result<IEnumerable<Reservation>>.Failure<IEnumerable<Reservation>>(new Error("repository-failure", "Repository failed to retrieve data")));

    var query = new ListQuery();

    // Act
    var result = await _handler.Handle(query);

    // Assert
    Assert.True(result.IsFailure);
    Assert.Equal("repository-failure", result.Error.Code);
  }
}