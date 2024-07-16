// using System.Data.Common;
using Microsoft.Data.SqlClient;
using Moq;
using Reservations.Core;
using Reservations.Core.ValueObjects;
using Reservations.Infrastructure.Persistence;
using Shared.Infrastructure.Persistence;

namespace Infrastructure.Tests;

public class ReservationRepositoryTests
{
  private readonly Mock<DbConnectionFactory> _connectionFactoryMock;
  private readonly Mock<SqlConnection> _dbConnectionMock;
  private readonly ReservationRepository _repository;

  public ReservationRepositoryTests()
  {
    _connectionFactoryMock = new Mock<DbConnectionFactory>();
    _dbConnectionMock = new Mock<SqlConnection>();
    _connectionFactoryMock.Setup(f => f.CreateConnection()).Returns(_dbConnectionMock.Object);
    _repository = new ReservationRepository(_connectionFactoryMock.Object);
  }

  [Fact]
  public async Task UpdateAsync_ReturnsSuccess_WhenReservationExists()
  {
    // Arrange
    var reservation = new Reservation(
        new ReservationId(Guid.NewGuid()),
        new ReservationStatus(ReservationStatusEnum.Open),
        new ReservationDate(DateTime.UtcNow),
        new ReservationOwnerName("John Doe"),
        new NumberOfGuests(2)
    );

    var commandMock = new Mock<SqlCommand>();
    commandMock.Setup(c => c.ExecuteNonQueryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

    _dbConnectionMock.Setup(c => c.CreateCommand()).Returns(commandMock.Object);

    // Act
    var result = await _repository.UpdateAsync(reservation);

    // Assert
    Assert.True(result.IsSuccess);
  }

  [Fact]
  public async Task GetByIdAsync_ReturnsReservation_WhenReservationExists()
  {
    // Arrange
    var reservationId = new ReservationId(Guid.NewGuid());
    var reservation = new Reservation(
        reservationId,
        new ReservationStatus(ReservationStatusEnum.Open),
        new ReservationDate(DateTime.UtcNow),
        new ReservationOwnerName("John Doe"),
        new NumberOfGuests(2)
    );

    var readerMock = new Mock<SqlDataReader>();
    readerMock.SetupSequence(r => r.ReadAsync(It.IsAny<CancellationToken>()))
              .ReturnsAsync(true)
              .ReturnsAsync(false);
    readerMock.Setup(r => r.GetString(It.IsAny<int>())).Returns(reservation.Id.Value);
    readerMock.Setup(r => r.GetString(It.IsAny<int>())).Returns(reservation.Status.ToString());
    readerMock.Setup(r => r.GetDateTime(It.IsAny<int>())).Returns(reservation.Date.Value);
    readerMock.Setup(r => r.GetInt32(It.IsAny<int>())).Returns(reservation.NumberOfGuests.Value);

    var commandMock = new Mock<SqlCommand>();
    commandMock.Setup(c => c.ExecuteReaderAsync(It.IsAny<CancellationToken>())).ReturnsAsync(readerMock.Object);

    _dbConnectionMock.Setup(c => c.CreateCommand()).Returns(commandMock.Object);

    // Act
    var result = await _repository.GetByIdAsync(reservationId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(reservationId, result.Id);
  }

  [Fact]
  public async Task MakeAsync_InsertsReservationSuccessfully()
  {
    // Arrange
    var reservation = new Reservation(
        new ReservationId(Guid.NewGuid()),
        new ReservationStatus(ReservationStatusEnum.Open),
        new ReservationDate(DateTime.UtcNow),
        new ReservationOwnerName("John Doe"),
        new NumberOfGuests(2)
    );

    var commandMock = new Mock<SqlCommand>();
    commandMock.Setup(c => c.ExecuteNonQueryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

    _dbConnectionMock.Setup(c => c.CreateCommand()).Returns(commandMock.Object);

    // Act
    await _repository.MakeAsync(reservation);

    // Assert
    commandMock.Verify(c => c.ExecuteNonQueryAsync(It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task ListAll_ReturnsAllReservations()
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

    var readerMock = new Mock<SqlDataReader>();
    readerMock.SetupSequence(r => r.ReadAsync(It.IsAny<CancellationToken>()))
              .ReturnsAsync(true)
              .ReturnsAsync(true)
              .ReturnsAsync(false);
    readerMock.SetupSequence(r => r.GetGuid(It.IsAny<int>()))
          .Returns(Guid.Parse(reservations[0].Id.Value))
          .Returns(Guid.Parse(reservations[1].Id.Value));
    readerMock.SetupSequence(r => r.GetString(It.IsAny<int>()))
              .Returns(reservations[0].Status.ToString())
              .Returns(reservations[1].Status.ToString());
    readerMock.SetupSequence(r => r.GetDateTime(It.IsAny<int>()))
              .Returns(reservations[0].Date.Value)
              .Returns(reservations[1].Date.Value);
    readerMock.SetupSequence(r => r.GetInt32(It.IsAny<int>()))
              .Returns(reservations[0].NumberOfGuests.Value)
              .Returns(reservations[1].NumberOfGuests.Value);

    var commandMock = new Mock<SqlCommand>();
    commandMock.Setup(c => c.ExecuteReaderAsync(It.IsAny<CancellationToken>())).ReturnsAsync(readerMock.Object);

    _dbConnectionMock.Setup(c => c.CreateCommand()).Returns(commandMock.Object);

    // Act
    var result = await _repository.ListAll();

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(reservations.Count, result.Value.Count());
  }
}