using Moq;
using Reservations.Core;
using Reservations.UseCases;

namespace UseCase.Tests;

public class MakeCommandHandlerTests
{
  private readonly Mock<IReservationRepository> _reservationRepositoryMock;
  private readonly MakeCommandHandler _handler;

  public MakeCommandHandlerTests()
  {
    _reservationRepositoryMock = new Mock<IReservationRepository>();
    _handler = new MakeCommandHandler(_reservationRepositoryMock.Object);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenReservationDateIsInThePast()
  {
    // Arrange
    var command = new MakeCommand(
        Guid.NewGuid().ToString(),
        DateTime.UtcNow.AddDays(-1),
        2,
        "John Doe"
    );
    // Act
    var handleResult = await _handler.Handle(command);

    // Assert
    Assert.True(handleResult.IsFailure);
    Assert.Equal("reservation-date-in-the-past", handleResult.Error.Code);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenReservationDateIsTooFarInTheFuture()
  {
    // Arrange
    var command = new MakeCommand(
        Guid.NewGuid().ToString(),
        DateTime.UtcNow.AddDays(31),
        2,
        "John Doe"
    );

    // Act
    var result = await _handler.Handle(command);

    // Assert
    Assert.True(result.IsFailure);
    Assert.Equal("reservation-date-too-far", result.Error.Code);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenReservationTimeIsOutsideAllowedRange()
  {
    // Arrange
    var command = new MakeCommand(
        Guid.NewGuid().ToString(),
        DateTime.UtcNow.AddDays(1).Date.AddHours(18), // 6 PM, outside allowed range
        2,
        "John Doe"
    );

    // Act
    var result = await _handler.Handle(command);

    // Assert
    Assert.True(result.IsFailure);
    Assert.Equal("reservation-time-outside-range", result.Error.Code);
  }

  [Fact]
  public async Task Handle_ReturnsSuccess_WhenReservationIsValid()
  {
    // Arrange
    var command = new MakeCommand(
        Guid.NewGuid().ToString(),
        DateTime.UtcNow.AddDays(1).Date.AddHours(20), // 8 PM, within allowed range
        2,
        "John Doe"
    );

    _reservationRepositoryMock.Setup(r => r.MakeAsync(It.IsAny<Reservation>()))
                              .Returns(Task.CompletedTask);

    // Act
    var result = await _handler.Handle(command);

    // Assert
    Assert.True(result.IsSuccess);
    _reservationRepositoryMock.Verify(r => r.MakeAsync(It.IsAny<Reservation>()), Times.Once);
  }
}