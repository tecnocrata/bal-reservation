

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Bus.Command;
using Shared.Core.Bus.Query;
using Shared.Infrastructure.Bus;
using Shared.Infrastructure.Bus.Command;
using Shared.Infrastructure.Bus.Query;
using SystemUsers.Core;
using SystemUsers.Infrastructre.Authentication;
using SystemUsers.Infrastructure;

namespace Shared.Infrastructure.Web;

public static class InfrastructureExtensions
{
  public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
  {
    // var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();
    // services.AddSingleton(jwtOptions);

    // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //     .AddJwtBearer(options =>
    //     {
    //         options.RequireHttpsMetadata = false;
    //         options.TokenValidationParameters = new TokenValidationParameters
    //         {
    //             ValidateIssuer = true,
    //             ValidIssuer = jwtOptions.Issuer,
    //             ValidateAudience = true,
    //             ValidAudience = jwtOptions.Audience,
    //             ValidateLifetime = true,
    //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey)),
    //             ValidateIssuerSigningKey = true,
    //         };
    //     });
    services.ConfigureOptions<JwtOptionsSetup>();
    services.ConfigureOptions<JwtBearerOptionsSetup>();
    services.AddAuthorization();
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();
  }

  public static void AddRestaurantServices(this IServiceCollection services)
  {
    services.AddScoped<ICommandBus, InMemoryCommandBus>();
    services.AddScoped<IQueryBus, InMemoryQueryBus>();
    services.AddScoped<ITokenProvider, JwtTokenProvider>();
    services.AddScoped<IUserRepository, UserRepository>();

    services.AddCommandServices(AssemblyLoader.GetInstance("SystemUsers.UseCases"));
    services.AddQueryServices(AssemblyLoader.GetInstance("SystemUsers.UseCases"));
  }
}