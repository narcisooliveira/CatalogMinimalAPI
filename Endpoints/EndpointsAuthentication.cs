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
                if (user.Nome is null || user.Senha is null)
                    return Results.BadRequest("Login inválido!");


                if (user.Nome != "narciso" || user.Senha != "p@ssWord")
                    return Results.Unauthorized();

                var token = tokenService.GenerateToken(app.Configuration["Jwt:Key"], app.Configuration["Jwt:Issuer"], app.Configuration["Jwt:Audience"]);

                return Results.Ok(new { token });
            }).WithTags("Authentication");
        }
    }
}
