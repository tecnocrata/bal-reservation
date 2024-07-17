using System.Data.Common;
using Shared.Infrastructure.Persistence;
using SystemUsers.Core;

namespace SystemUsers.Infrastructure;

public class UserRepository : IUserRepository
{
  private readonly DbConnectionFactory _connectionFactory;

  public UserRepository(DbConnectionFactory connectionFactory)
  {
    _connectionFactory = connectionFactory;
  }

  public async Task<User?> CheckUserCredentials(string userName, string password)
  {
    using (var connection = _connectionFactory.CreateConnection())
    {
      await connection.OpenAsync();

      string query = "SELECT UserName, Password FROM Users WHERE UserName = @UserName";
      using (var command = connection.CreateCommand())
      {
        command.CommandText = query;
        command.Parameters.Add(CreateParameter(command, "@UserName", userName));

        using (var reader = await command.ExecuteReaderAsync())
        {
          if (await reader.ReadAsync())
          {
            var storedUserName = reader.GetString(reader.GetOrdinal("UserName"));
            var storedHashedPassword = reader.GetString(reader.GetOrdinal("Password"));

            if (PasswordHasher.VerifyPassword(password, storedHashedPassword))
            {
              return new User(storedUserName, storedHashedPassword);
            }
          }
        }
      }
    }

    return null;
  }

  private DbParameter CreateParameter(DbCommand command, string name, object value)
  {
    var parameter = command.CreateParameter();
    parameter.ParameterName = name;
    parameter.Value = value;
    return parameter;
  }
}