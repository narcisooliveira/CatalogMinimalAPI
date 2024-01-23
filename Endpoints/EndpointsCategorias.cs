using CatalogMinimalAPI.Context;
using CatalogMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogMinimalAPI.Endpoints
{
    public static class EndpointsCategorias
    {
        public static void MapEndpointsCategorias(this WebApplication app)
        {
            // Endpoints of categorias
            app.MapGet("/categorias", async (CatalogContext context) => await context.Categorias.ToListAsync()).RequireAuthorization().WithTags("Categorias");

            app.MapGet("/categorias/{id}", async (CatalogContext context, int id) =>
            {
                IResult result = await context.Categorias.FindAsync(id) is Categoria category ? Results.Ok(category) : Results.NotFound();

                return result;
            }).WithTags("Categorias"); ;

            app.MapPost("/categorias", async (CatalogContext context, Categoria category) =>
            {
                await context.Categorias.AddAsync(category);
                await context.SaveChangesAsync();
                return Results.Created($"/categorias/{category.Id}", category);
            }).WithTags("Categorias"); ;

            app.MapPut("/categorias/{id}", async (CatalogContext context, int id, Categoria category) =>
            {
                if (id != category.Id)
                    return Results.BadRequest();

                context.Entry(category).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Results.Ok(category);
            }).WithTags("Categorias"); ;

            app.MapDelete("/categorias/{id}", async (CatalogContext context, int id) =>
            {
                var category = await context.Categorias.FindAsync(id);
                if (category is null)
                    return Results.NotFound();

                context.Categorias.Remove(category);
                await context.SaveChangesAsync();
                return Results.NoContent();
            }).WithTags("Categorias"); ;
        }

    }
}
