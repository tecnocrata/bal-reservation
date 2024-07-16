using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure;
using SystemUsers.Core;

namespace SystemUsers.Infrastructre.Authentication;
public class JwtTokenProvider : ITokenProvider
{
  private readonly JwtOptions options;

  public JwtTokenProvider(IOptions<JwtOptions> options)
  {
    this.options = options.Value ?? throw new ArgumentNullException(nameof(options));
  }

  public string GenerateToken(User user)
  {
    var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.UserName)
        };

    var signingCredentials = new SigningCredentials(
        new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(options.SecretKey!)),
        SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        options.Issuer,
        options.Audience,
        claims,
        null,
        DateTime.UtcNow.AddHours(1),
        signingCredentials);

    string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

    return tokenValue;
  }
}