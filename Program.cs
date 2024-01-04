using CatalogMinimalAPI.AppServicesExtensions;
using CatalogMinimalAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwager()
       .AddPersistence()
       .Services.AddCors();
builder.AddAuthenticationJwt();

// Configure the HTTP request pipeline. 
var app = builder.Build();

// Call the extension methods from Endpoints 
app.MapEndpointsAuthentication();
app.MapEndpointsCategorias();
app.MapEndpointsProdutos();

var environment = app.Environment;
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();