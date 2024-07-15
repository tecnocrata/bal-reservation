using Api.Endpoints;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bla API", Version = "v1" });
});

WebApplication app = builder.Build();

app.MapReservationEndpoints();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bla API V1"));
}

// app.UseAuthentication();

// app.UseAuthorization();

await app.RunAsync();
