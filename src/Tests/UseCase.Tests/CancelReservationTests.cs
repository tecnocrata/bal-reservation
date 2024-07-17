using System;
using System.Threading.Tasks;
using Moq;
using Reservations.Core;
using Reservations.Core.ValueObjects;
using Reservations.UseCases;
using Shared.Core.Bus.Command;
using Shared.Core.Results;
using Xunit;

namespace UseCase.Tests;
public class CancelCommandHandlerTests
{
  private readonly Mock<IReservationRepository> _reservationRepositoryMock;
  private readonly CancelCommandHandler _handler;

  public CancelCommandHandlerTests()
  {
    _reservationRepositoryMock = new Mock<IReservationRepository>();
    _handler = new CancelCommandHandler(_reservationRepositoryMock.Object);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenReservationDoesNotExist()
  {
    // Arrange
    var command = new CancelCommand { ReservationId = Guid.NewGuid() };
    _reservationRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<ReservationId>()))
                              .ReturnsAsync((Reservation?)null);

    // Act
    var result = await _handler.Handle(command);

    // Assert
    Assert.True(result.IsFailure);
    Assert.Equal("reservation-not-found", result.Error.Code);
  }

  [Fact]
  public async Task Handle_CancelsReservation_WhenReservationExists()
  {
    // Arrange
    var reservation = new Reservation(
        new ReservationId(Guid.NewGuid()),
        new ReservationStatus(ReservationStatusEnum.Open),
        new ReservationDate(DateTime.UtcNow),
        new ReservationOwnerName("John Doe"),
        new NumberOfGuests(2)
    );

    var command = new CancelCommand { ReservationId = reservation.Id.ValueUuid };

    _reservationRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<ReservationId>()))
                              .ReturnsAsync(reservation);
    _reservationRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Reservation>()))
                              .ReturnsAsync(Result.Success());

    // Act
    var result = await _handler.Handle(command);

    // Assert
    Assert.True(result.IsSuccess);
    _reservationRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Reservation>(res => res.Status.Value == ReservationStatusEnum.Cancelled)), Times.Once);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenUpdateFails()
  {
    // Arrange
    var reservation = new Reservation(
        new ReservationId(Guid.NewGuid()),
        new ReservationStatus(ReservationStatusEnum.Open),
        new ReservationDate(DateTime.UtcNow),
        new ReservationOwnerName("John Doe"),
        new NumberOfGuests(2)
    );

    var command = new CancelCommand { ReservationId = reservation.Id.ValueUuid };

    _reservationRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<ReservationId>()))
                              .ReturnsAsync(reservation);
    _reservationRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Reservation>()))
                              .ReturnsAsync(Result.Failure(new Error("update-failure", "Failed to update reservation")));

    // Act
    var result = await _handler.Handle(command);

    // Assert
    Assert.True(result.IsFailure);
    Assert.Equal("update-failure", result.Error.Code);
  }
}

