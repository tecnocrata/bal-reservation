using System;
using System.Threading.Tasks;
using Moq;
using Reservations.Core;
using Reservations.Core.ValueObjects;
using Reservations.UseCases;
using Shared.Core.Bus.Query;
using Shared.Core.Results;
using Xunit;

namespace UseCase.Tests;
public class DetailQueryHandlerTests
{
  private readonly Mock<IReservationRepository> _reservationRepositoryMock;
  private readonly DetailQueryHandler _handler;

  public DetailQueryHandlerTests()
  {
    _reservationRepositoryMock = new Mock<IReservationRepository>();
    _handler = new DetailQueryHandler(_reservationRepositoryMock.Object);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenReservationDoesNotExist()
  {
    // Arrange
    var query = new DetailQuery { ReservationId = Guid.NewGuid() };
    _reservationRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<ReservationId>()))
                              .ReturnsAsync((Reservation?)null);

    // Act
    var result = await _handler.Handle(query);

    // Assert
    Assert.True(result.IsFailure);
    Assert.Equal("reservation-not-found", result.Error.Code);
  }

  [Fact]
  public async Task Handle_ReturnsSuccess_WithReservationDetails_WhenReservationExists()
  {
    // Arrange
    var reservation = new Reservation(
        new ReservationId(Guid.NewGuid()),
        new ReservationStatus(ReservationStatusEnum.Open),
        new ReservationDate(DateTime.UtcNow),
        new ReservationOwnerName("John Doe"),
        new NumberOfGuests(2)
    );

    var query = new DetailQuery { ReservationId = reservation.Id.ValueUuid };

    _reservationRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<ReservationId>()))
                              .ReturnsAsync(reservation);

    // Act
    var result = await _handler.Handle(query);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(reservation.Id.ValueUuid, result.Value.Id);
    Assert.Equal(reservation.Status.ToString(), result.Value.Status);
    Assert.Equal(reservation.Date.Value, result.Value.Date);
    Assert.Equal(reservation.Name.Value, result.Value.Name);
  }
}

