using Shared.Core.Bus.Query;
using Shared.Core.Results;
using SystemUsers.Core;

namespace SytemUsers.UseCases.Login;

public class LoginQueryHandler : IQueryHandler<LoginQuery, Result<string>>
{
  private readonly IUserRepository _userRepository;
  private readonly ITokenProvider _tokenProvider;

  public LoginQueryHandler(IUserRepository userRepository, ITokenProvider tokenProvider)
  {
    _userRepository = userRepository;
    _tokenProvider = tokenProvider;
  }

  public async Task<Result<string>> Handle(LoginQuery query)
  {
    var user = await _userRepository.CheckUserCredentials(query.UserName!, query.Password!);
    if (user == null)
    {
      return Result.Failure<string>(new Error("user-not-found", "User not found"));
    }

    var token = _tokenProvider.GenerateToken(user);
    return token!;
  }
}