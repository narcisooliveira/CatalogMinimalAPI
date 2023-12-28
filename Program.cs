using CatalogMinimalAPI.Context;
using CatalogMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CatalogContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Define the endpoints of the API
app.MapGet("/", () => "Catalog API - 2023").ExcludeFromDescription();

// Endpoints of categorias
app.MapGet("/categorias", async (CatalogContext context) => await context.Categorias.ToListAsync());

app.MapGet("/categorias/{id}", async (CatalogContext context, int id) =>
{
    IResult result = await context.Categorias.FindAsync(id) is Categoria category ? Results.Ok(category) : Results.NotFound();

    return result;
});

app.MapPost("/categorias", async (CatalogContext context, Categoria category) =>
{
    await context.Categorias.AddAsync(category);
    await context.SaveChangesAsync();
    return Results.Created($"/categorias/{category.Id}", category);
});

app.MapPut("/categorias/{id}", async (CatalogContext context, int id, Categoria category) =>
{
    if (id != category.Id)
        return Results.BadRequest();

    context.Entry(category).State = EntityState.Modified;
    await context.SaveChangesAsync();
    return Results.Ok(category);
});

app.MapDelete("/categorias/{id}", async (CatalogContext context, int id) =>
{
    var category = await context.Categorias.FindAsync(id);
    if (category is null)
        return Results.NotFound();

    context.Categorias.Remove(category);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

// Endpoints of produtos
app.MapGet("/produtos", async (CatalogContext context) => await context.Produtos.ToListAsync());

app.MapGet("/produtos/{id}", async (CatalogContext context, int id) =>
{
    IResult result = await context.Produtos.FindAsync(id) is Produto product ? Results.Ok(product) : Results.NotFound();
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

    var existingProduct = await context.Produtos.FindAsync(id);

    if (existingProduct is null)
        return Results.NotFound();

    context.Entry(product).State = EntityState.Modified;
    await context.SaveChangesAsync();
    return Results.Ok(product);
});

app.MapDelete("/produtos/{id}", async (CatalogContext context, int id) =>
{
    var product = await context.Produtos.FindAsync(id);
    if (product == null)
    {
        return Results.NotFound();
    }

    context.Produtos.Remove(product);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();