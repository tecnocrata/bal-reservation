namespace SystemUsers.Core;

public interface IUserRepository
{
  Task<User> CheckUserCredentials(string userName, string password);
}