using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.Common;

namespace Shared.Infrastructure.Persistence;
public class DbConnectionFactory
{
  private readonly IConfiguration _configuration;
  private readonly bool _useSqlite;

  public DbConnectionFactory(IConfiguration configuration, bool useSqlite = false)
  {
    _configuration = configuration;
    _useSqlite = useSqlite;
  }

  public DbConnection CreateConnection()
  {
    if (_useSqlite)
    {
      var connection = new SqliteConnection("DataSource=:memory:");
      connection.Open();

      using (var command = connection.CreateCommand())
      {
        command.CommandText = @"
                        CREATE TABLE Reservations (
                            Id TEXT PRIMARY KEY,
                            Status TEXT NOT NULL,
                            Date TEXT NOT NULL,
                            Name TEXT NOT NULL,
                            NumberOfGuests INTEGER NOT NULL
                        );";
        command.ExecuteNonQuery();

        command.CommandText = @"
                        CREATE TABLE Users (
                            UserName TEXT PRIMARY KEY,
                            Password TEXT NOT NULL
                        );";
        command.ExecuteNonQuery();
      }

      return connection;
    }
    else
    {
      var connectionString = _configuration.GetConnectionString("BlaConnection");
      return new SqlConnection(connectionString);
    }
  }
}