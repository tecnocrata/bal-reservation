﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Infrastructure.Web;

public class JwtBearerOptionsSetup : IPostConfigureOptions<JwtBearerOptions>
{
  private readonly JwtOptions _jwtOptions;

  public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
  {
    _jwtOptions = jwtOptions.Value;
  }

  public void PostConfigure(string? name, JwtBearerOptions options)
  {
    options.TokenValidationParameters.ValidIssuer = _jwtOptions.Issuer;
    options.TokenValidationParameters.ValidAudience = _jwtOptions.Audience;
    if (_jwtOptions.SecretKey is not null)
    {
      options.TokenValidationParameters.IssuerSigningKey =
          new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
    }
  }
}
