using Microsoft.EntityFrameworkCore;
using TransitStopApp.Server.Interfaces;
using TransitStopApp.Server.Utility;

var builder = WebApplication.CreateBuilder(args);

// Fetch configuration values and validate
var dbConnStr = builder.Configuration["TransitStopDbConnString"]
    ?? throw new InvalidOperationException("TransitStopDbConnString is not configured."); ;
var clientOrigin = builder.Configuration["ClientOrigin"]
    ?? throw new InvalidOperationException("ClientOrigin is not configured."); ;

// Configure CORS for client
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(clientOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Initialize and seed DB
// in prod, could use an IHostedService to avoid startup delays / unhandled errors
await TransitStopDbInitializer.Initialize(dbConnStr);

// Dependency Injection
builder.Services.AddDbContextFactory<TransitStopDbContext>(options =>
    options.UseSqlite(dbConnStr));

builder.Services.AddTransient<ITransitStopOperations>(sp =>
{
    var dbFactory = sp.GetRequiredService<IDbContextFactory<TransitStopDbContext>>();
    var db = dbFactory.CreateDbContext();
    return new TransitStopOperations(db);
});

builder.Services.AddSingleton<IMinuteOfDayConverter, MinuteOfDayConverter>();
builder.Services.AddSingleton<ICurrentTimeFetcher, CurrentTimeFetcherPst>();

// Add services for controllers
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors();
app.MapControllers();
app.Run();