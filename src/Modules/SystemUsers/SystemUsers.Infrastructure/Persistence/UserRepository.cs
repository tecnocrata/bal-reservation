using SystemUsers.Core;

namespace SystemUsers.Infrastructure;

public class UserRepository : IUserRepository
{
  // private readonly SystemUsersDbContext _dbContext;

  // public UserRepository(SystemUsersDbContext dbContext)
  // {
  //   _dbContext = dbContext;
  // }
  public Task<User> CheckUserCredentials(string userName, string password)
  {
    return Task.FromResult(new User(userName, password));
  }
}