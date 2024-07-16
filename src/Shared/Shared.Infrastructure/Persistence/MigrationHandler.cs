using System.Reflection;
using System.Text.RegularExpressions;
using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Shared.Infrastructure.Persistence;

public static class MigrationHandler
{
  public static void Migrate(IConfiguration configuration)
  {
    var connectionString = configuration["ConnectionStrings:BlaConnection"];
    EnsureDatabase.For.SqlDatabase(connectionString);

    var upgrader =
        DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .WithTransaction()
            .LogToConsole()
            .Build();

    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(result.Error);
      Console.ResetColor();
#if DEBUG
      Console.ReadLine();
#endif
      return;
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Database migration successful!");
    Console.ResetColor();
  }
}