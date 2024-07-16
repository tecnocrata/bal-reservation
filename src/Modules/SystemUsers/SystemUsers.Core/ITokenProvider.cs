namespace SystemUsers.Core;

public interface ITokenProvider
{
  Task<string> GenerateToken(User user);
}