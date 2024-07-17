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
  private readonly DbConnectionFactory _connectionFactory;
  private readonly ReservationRepository _repository;

  public ReservationRepositoryTests()
  {
    _connectionFactory = new DbConnectionFactory(null!, useSqlite: true);
    _repository = new ReservationRepository(_connectionFactory);
  }

  [Fact(Skip = "This test is temporarily disabled")]
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

    await _repository.MakeAsync(reservation);

    reservation.Cancel();

    // Act
    var result = await _repository.UpdateAsync(reservation);

    // Assert
    Assert.True(result.IsSuccess);
  }

  [Fact(Skip = "This test is temporarily disabled")]
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

    await _repository.MakeAsync(reservation);

    // Act
    var result = await _repository.GetByIdAsync(reservationId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(reservationId, result.Id);
  }

  [Fact(Skip = "This test is temporarily disabled")]
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

    // Act
    await _repository.MakeAsync(reservation);

    // Assert
    var result = await _repository.GetByIdAsync(reservation.Id);
    Assert.NotNull(result);
  }

  [Fact(Skip = "This test is temporarily disabled")]
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

    foreach (var reservation in reservations)
    {
      await _repository.MakeAsync(reservation);
    }

    // Act
    var result = await _repository.ListAll();

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(reservations.Count, result.Value.Count());
  }
}