using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace Shared.Infrastructure.Persistence;
public class DbConnectionFactory
{
  private readonly IConfiguration _configuration;

  public DbConnectionFactory(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public SqlConnection CreateConnection()
  {
    var connectionString = _configuration.GetConnectionString("BlaConnection");
    return new SqlConnection(connectionString);
  }
}