using CatalogMinimalAPI.Context;
using CatalogMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogMinimalAPI.Endpoints
{
    public static class EndpointsProdutos
    {
        public static void MapEndpointsProdutos(this WebApplication app)
        {
            // Endpoints of produtos
            app.MapGet("/produtos", async (CatalogContext context) => await context.Produtos.ToListAsync()).RequireAuthorization().WithTags("Produtos");

            app.MapGet("/produtos/{id}", async (CatalogContext context, int id) =>
            {
                IResult result = await context.Produtos.FindAsync(id) is Produto product ? Results.Ok(product) : Results.NotFound();

                return result;
            });

            app.MapPost("/produtos", async (CatalogContext context, Produto product) =>
            {
                await context.Produtos.AddAsync(product);
                await context.SaveChangesAsync();
                return Results.Created($"/produtos/{product.Id}", product);
            });

            app.MapPut("/produtos/{id}", async (CatalogContext context, int id, Produto product) =>
            {
                if (id != product.Id)
                    return Results.BadRequest();

                context.Entry(product).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Results.Ok(product);
            });

            app.MapDelete("/produtos/{id}", async (CatalogContext context, int id) =>
            {
                var product = await context.Produtos.FindAsync(id);
                if (product is null)
                    return Results.NotFound();

                context.Produtos.Remove(product);
                await context.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
