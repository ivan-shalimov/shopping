using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Server.Endpoints;
using Shopping.Server.Extensions;
using Shopping.Server.Services;
using Shopping.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetry(builder.Configuration["OtplCollectorEndpoint"]);

var connectionStr = builder.Configuration.GetConnectionString("Shopping");
builder.Services.AddSqlServer<ShoppingDbContext>(connectionStr);

builder.Services.RegisterServices();
builder.Services.RegisterMediatR();
builder.Services.RegisterMediatrServices();
builder.Services.AddCors();
builder.Services.AddHostedService<BackgroundTaskProcessor>();

Console.WriteLine("[Starting service]: Build");
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("[Starting service]: Database Migrate Starting");
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingDbContext>();
    dbContext.Database.Migrate();
    Console.WriteLine("[Starting service]: Database Migrate Finished");

    app.UseOpenTelemetryPrometheusScrapingEndpoint();
}

app.UseGlobalExceptionHandling();

app.UseCors(conf => conf.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapEndpoints();
Console.WriteLine("[Starting service]: Run");
app.Run();

public partial class Program
{ }