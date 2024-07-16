namespace SystemUsers.Core;
public class User
{
  public Guid Id { get; private set; }
  public string UserName { get; private set; }
  public string Password { get; private set; }

  public User(string userName, string password)
  {
    Id = Guid.NewGuid();
    UserName = userName;
    Password = password;
  }
}