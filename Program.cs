using CatalogMinimalAPI.Context;
using CatalogMinimalAPI.Models;
using CatalogMinimalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CatalogContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))           
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Endpoints of authentication
app.MapPost("/login", [AllowAnonymous] (User user, ITokenService tokenService) =>
{
    if (user.Nome is null || user.Senha is null)
        return Results.BadRequest("Login inválido!");


    if (user.Nome != "narciso" || user.Senha != "p@ssWord")
        return Results.Unauthorized();

    var token = tokenService.GenerateToken(app.Configuration["Jwt:Key"], app.Configuration["Jwt:Issuer"], app.Configuration["Jwt:Audience"]);
    
    return Results.Ok(new { token });
});

// Define the endpoints of the API
app.MapGet("/", () => "Catalog API - 2023").ExcludeFromDescription();

// Endpoints of categorias
app.MapGet("/categorias", async (CatalogContext context) => await context.Categorias.ToListAsync()).RequireAuthorization();

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
app.MapGet("/produtos", async (CatalogContext context) => await context.Produtos.ToListAsync()).RequireAuthorization();

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

    var existingProduct = await context.Produtos.FindAsync(id);

    if (existingProduct is null)
        return Results.NotFound();

    existingProduct.Id = product.Id;
    existingProduct.Nome = product.Nome;
    existingProduct.Descricao = product.Descricao;
    existingProduct.Preco = product.Preco;
    existingProduct.ImagemUrl = product.ImagemUrl;
    existingProduct.DataCompra = existingProduct.DataCompra;
    existingProduct.Quantidade = product.Quantidade;
    existingProduct.CategoriaId = product.CategoriaId;
    
    await context.SaveChangesAsync();
    return Results.Ok(product);
});

app.MapDelete("/produtos/{id}", async (CatalogContext context, int id) =>
{
    var product = await context.Produtos.FindAsync(id);
    if (product == null)
        return Results.NotFound();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();