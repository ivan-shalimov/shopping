using Shopping.DataAccess;
using Shopping.Server;
using Shopping.Services;

var builder = WebApplication
    .CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var settings = builder.Configuration.Get<AppSettigns>();
builder.Services.AddSingleton<AppSettigns>(settings);

var connectionStr = builder.Configuration.GetConnectionString("Shopping");
builder.Services.AddSqlServer<ShoppingDbContext>(connectionStr);

builder.Services.RegisterMediatR();
builder.Services.RegisterMediatrServices();
builder.Services.AddCors();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // todo implement special handling
    app.UseExceptionHandler("/Error");
}

app.UseRouting();
app.UseCors(conf=>conf.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();
app.Run();

public partial class Program
{ }