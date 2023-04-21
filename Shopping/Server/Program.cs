using Shopping.DataAccess;
using Shopping.SeriGraylog;
using Shopping.Server;
using Shopping.Server.Extensions;
using Shopping.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSeriGraylog();

var settings = builder.Configuration.Get<AppSettigns>();
builder.Services.AddSingleton<AppSettigns>(settings);

var connectionStr = builder.Configuration.GetConnectionString("Shopping");
builder.Services.AddSqlServer<ShoppingDbContext>(connectionStr);

builder.Services.RegisterMediatR();
builder.Services.RegisterMediatrServices();
builder.Services.AddCors();
builder.Services.AddControllers();

var app = builder.Build();

app.UseGlobalExceptionHandling();

app.UseRouting();
app.UseCors(conf => conf.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();
app.Run();

public partial class Program
{ }