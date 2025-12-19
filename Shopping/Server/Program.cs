using Shopping.DataAccess;
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
builder.Services.AddControllers();
builder.Services.AddHostedService<BackgroundTaskProcessor>();

Console.WriteLine("[Starting service]: Build");
var app = builder.Build();
app.UseGlobalExceptionHandling();

app.UseRouting();
app.UseCors(conf => conf.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

Console.WriteLine("[Starting service]: Run");
app.Run();

public partial class Program
{ }