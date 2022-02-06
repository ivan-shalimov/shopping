using MediatR;
using Shopping.DataAccess;
using Shopping.Models.Results;
using Shopping.Requests;
using Shopping.Services.Handlers;
using Shopping.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

var settings = builder.Configuration.Get<AppSettigns>();
builder.Services.AddSingleton(settings);

var connectionStr = builder.Configuration.GetConnectionString("Shopping");
builder.Services.AddSqlServer<ShoppingDbContext>(connectionStr);

builder.Services.AddScoped<IRequestHandler<GetPurchaseStatistic, PurchaseStatistic>, GetPurchaseStatisticHandler>();
builder.Services.AddScoped<IRequestHandler<GetPurchases, PurchaseItem[]>, GetPurchasesHandler>();
builder.Services.AddScoped<IRequestHandler<AddPurchase, Unit>, AddPurchaseHandler>();
builder.Services.AddScoped<IRequestHandler<DeletePurchase, Unit>, DeletePurchaseHandler>();

builder.Services.AddTransient<ServiceFactory>(p => p.GetService);
builder.Services.AddTransient<IMediator, Mediator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();