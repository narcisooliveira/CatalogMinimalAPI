using CatalogMinimalAPI.AppServicesExtensions;
using CatalogMinimalAPI.Endpoints;
using Hangfire;
using Hangfire.Storage.SQLite;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwager()
       .AddPersistence()
       .Services.AddCors();
builder.AddAuthenticationJwt();

// Add hangfire services and configure the storage to use.
builder.Services.AddHangfire(config =>
{
    config
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Adding quantity retry attempts
GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
{
    Attempts = 3,
    DelaysInSeconds = new int[] { 300 }
});

// Configure the HTTP request pipeline. 
var app = builder.Build();

// Configure the hangfire server
app.UseHangfireServer();

// Configure the hangfire dashboard
app.UseHangfireDashboard();

// Fire and forget
BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));

// Delayed
BackgroundJob.Schedule(() => Console.WriteLine("Delayed!"), TimeSpan.FromDays(7));

// Recurring
RecurringJob.AddOrUpdate(
"Meu job recorrente",
    () => Console.WriteLine((new Random().Next(1, 200) % 2 == 0)
        ? "Job recorrente gerou um número par"
        : "Job recorrente gerou um número ímpar"),
    Cron.Minutely,
    TimeZoneInfo.Local);

// Continuations
var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Job fire-and-forget pai!"));
BackgroundJob.ContinueJobWith(
    jobId,
    () => Console.WriteLine($"Job continuation! (Job pai: {jobId})"));

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