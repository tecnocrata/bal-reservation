

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Bus.Command;
using Shared.Core.Bus.Query;
using Shared.Infrastructure.Bus;
using Shared.Infrastructure.Bus.Command;
using Shared.Infrastructure.Bus.Query;
using Shared.Infrastructure.Persistence;
using SystemUsers.Core;
using SystemUsers.Infrastructre.Authentication;
using SystemUsers.Infrastructure;

namespace Shared.Infrastructure.Web;

public static class InfrastructureExtensions
{
  public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
  {
    services.ConfigureOptions<JwtOptionsSetup>();
    services.ConfigureOptions<JwtBearerOptionsSetup>();
    services.AddAuthorization();
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();
  }

  public static void AddRestaurantServices(this IServiceCollection services)
  {
    services.AddSingleton<DbConnectionFactory>();
    services.AddScoped<ICommandBus, InMemoryCommandBus>();
    services.AddScoped<IQueryBus, InMemoryQueryBus>();
    services.AddScoped<ITokenProvider, JwtTokenProvider>();
    services.AddScoped<IUserRepository, UserRepository>();

    services.AddCommandServices(AssemblyLoader.GetInstance("SystemUsers.UseCases"));
    services.AddQueryServices(AssemblyLoader.GetInstance("SystemUsers.UseCases"));
  }

  public static void UseRestaurantServices(this IApplicationBuilder app, IConfiguration configuration)
  {
    MigrationHandler.Migrate(configuration);
    // app.UseAuthentication();
    // app.UseAuthorization();
  }
}