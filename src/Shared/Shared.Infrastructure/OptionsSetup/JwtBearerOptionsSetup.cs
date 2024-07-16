using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Infrastructure;

public class JwtBearerOptionsSetup : IPostConfigureOptions<JwtBearerOptions>
{
  private readonly JwtOptions options;

  public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
  {
    options = jwtOptions.Value;
  }

  public void PostConfigure(string? name, JwtBearerOptions options)
  {
    options.TokenValidationParameters.ValidIssuer = this.options.Issuer;
    options.TokenValidationParameters.ValidAudience = this.options.Audience;

    options.TokenValidationParameters.IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.SecretKey!));
  }
}
