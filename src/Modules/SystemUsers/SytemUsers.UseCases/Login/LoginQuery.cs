using Shared.Core.Bus.Query;

namespace SytemUsers.UseCases.Login;
public class LoginQuery : IQuery
{
  public string? UserName { get; set; }
  public string? Password { get; set; }
}