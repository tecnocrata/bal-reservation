using Microsoft.Data.SqlClient;
using Reservations.Core;
using Reservations.Core.ValueObjects;
using Shared.Core.Results;
using Shared.Infrastructure.Persistence;

namespace Reservations.Infrastructure.Persistence;

public class ReservationRepository : IReservationRepository
{
  private readonly DbConnectionFactory _connectionFactory;

  public ReservationRepository(DbConnectionFactory connectionFactory)
  {
    _connectionFactory = connectionFactory;
  }

  public async Task<Result> UpdateAsync(Reservation reservation)
  {
    using (var connection = _connectionFactory.CreateConnection())
    {
      await connection.OpenAsync();

      string query = @"
                UPDATE Reservations
                SET Status = @Status, Date = @Date, Name = @Name, NumberOfGuests = @NumberOfGuests
                WHERE Id = @Id";

      using (var command = new SqlCommand(query, connection))
      {
        command.Parameters.AddWithValue("@Id", reservation.Id.Value);
        command.Parameters.AddWithValue("@Status", reservation.Status.ToString());
        command.Parameters.AddWithValue("@Date", reservation.Date.Value);
        command.Parameters.AddWithValue("@Name", reservation.Name.Value);
        command.Parameters.AddWithValue("@NumberOfGuests", reservation.NumberOfGuests.Value);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0 ? Result.Success() : Result.Failure(new Error("reservation-not-found", "Reservation not found"));
      }
    }
  }

  public async Task<Reservation?> GetByIdAsync(ReservationId id)
  {
    using (var connection = _connectionFactory.CreateConnection())
    {
      await connection.OpenAsync();

      string query = "SELECT Id, Status, Date, Name, NumberOfGuests FROM Reservations WHERE Id = @Id";
      using (var command = new SqlCommand(query, connection))
      {
        command.Parameters.AddWithValue("@Id", id.Value);

        using (var reader = await command.ExecuteReaderAsync())
        {
          if (await reader.ReadAsync())
          {
            var reservationId = new ReservationId(reader.GetGuid(reader.GetOrdinal("Id")));
            var status = new ReservationStatus((ReservationStatusEnum)Enum.Parse(typeof(ReservationStatusEnum), reader.GetString(reader.GetOrdinal("Status"))));
            var date = new ReservationDate(reader.GetDateTime(reader.GetOrdinal("Date")));
            var name = new ReservationOwnerName(reader.GetString(reader.GetOrdinal("Name")));
            var numberOfGuests = new NumberOfGuests(reader.GetInt32(reader.GetOrdinal("NumberOfGuests")));

            return new Reservation(reservationId, status, date, name, numberOfGuests);
          }
        }
      }
    }

    return null;
  }

  public async Task MakeAsync(Reservation reservation)
  {
    using (var connection = _connectionFactory.CreateConnection())
    {
      await connection.OpenAsync();

      string query = @"
                INSERT INTO Reservations (Id, Status, Date, Name, NumberOfGuests)
                VALUES (@Id, @Status, @Date, @Name, @NumberOfGuests)";

      using (var command = new SqlCommand(query, connection))
      {
        command.Parameters.AddWithValue("@Id", reservation.Id.Value);
        command.Parameters.AddWithValue("@Status", reservation.Status.ToString());
        command.Parameters.AddWithValue("@Date", reservation.Date.Value);
        command.Parameters.AddWithValue("@Name", reservation.Name.Value);
        command.Parameters.AddWithValue("@NumberOfGuests", reservation.NumberOfGuests.Value);

        await command.ExecuteNonQueryAsync();
      }
    }
  }

  public async Task<Result<IEnumerable<Reservation>>> ListAll()
  {
    var reservations = new List<Reservation>();

    using (var connection = _connectionFactory.CreateConnection())
    {
      await connection.OpenAsync();

      string query = "SELECT Id, Status, Date, Name, NumberOfGuests FROM Reservations";
      using (var command = new SqlCommand(query, connection))
      {
        using (var reader = await command.ExecuteReaderAsync())
        {
          while (await reader.ReadAsync())
          {
            var reservationId = new ReservationId(reader.GetGuid(reader.GetOrdinal("Id")));
            var status = new ReservationStatus((ReservationStatusEnum)Enum.Parse(typeof(ReservationStatusEnum), reader.GetString(reader.GetOrdinal("Status"))));
            var date = new ReservationDate(reader.GetDateTime(reader.GetOrdinal("Date")));
            var name = new ReservationOwnerName(reader.GetString(reader.GetOrdinal("Name")));
            var numberOfGuests = new NumberOfGuests(reader.GetInt32(reader.GetOrdinal("NumberOfGuests")));

            var reservation = new Reservation(reservationId, status, date, name, numberOfGuests);
            reservations.Add(reservation);
          }
        }
      }
    }

    return Result.Success(reservations.AsEnumerable());
  }
}