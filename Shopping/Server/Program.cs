using Shopping.DataAccess;
using Shopping.Server;
using Shopping.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var settings = builder.Configuration.Get<AppSettigns>();
builder.Services.AddSingleton(settings);

var connectionStr = builder.Configuration.GetConnectionString("Shopping");
builder.Services.AddSqlServer<ShoppingDbContext>(connectionStr);

builder.Services.RegisterMediatR();
builder.Services.RegisterMediatrServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

public partial class Program
{ }