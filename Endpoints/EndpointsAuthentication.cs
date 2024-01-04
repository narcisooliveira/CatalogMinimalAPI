using CatalogMinimalAPI.Models;
using CatalogMinimalAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace CatalogMinimalAPI.Endpoints
{
    public static class EndpointsAuthentication
    {
        public static void MapEndpointsAuthentication(this WebApplication app)
        {
            // Endpoints of authentication
            app.MapPost("/login", [AllowAnonymous] (User user, ITokenService tokenService) =>
            {
                if (user.Name is null || user.Password is null)
                    return Results.BadRequest("Login inválido!");


                if (user.Name != "narciso" || user.Password != "p@ssWord")
                    return Results.Unauthorized();

                var token = tokenService.GenerateToken(app.Configuration["Jwt:Key"], app.Configuration["Jwt:Issuer"], app.Configuration["Jwt:Audience"], user);

                return Results.Ok(new { token });
            }).WithTags("Authentication");
        }
    }
}
