namespace SystemUsers.Core;

public interface ITokenProvider
{
  string GenerateToken(User user);
}