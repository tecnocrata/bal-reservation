

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Web;

public static class JwtExtensions
{
  public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
  {
    services.ConfigureOptions<JwtOptionsSetup>();
    services.ConfigureOptions<JwtBearerOptionsSetup>();

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();
  }
}