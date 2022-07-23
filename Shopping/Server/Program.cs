using Microsoft.AspNetCore.ResponseCompression;
using MediatR;
using Shopping.DataAccess;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using Shopping.Services.Handlers;
using Shopping.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var settings = builder.Configuration.Get<AppSettigns>();
builder.Services.AddSingleton(settings);

var connectionStr = builder.Configuration.GetConnectionString("Shopping");
builder.Services.AddSqlServer<ShoppingDbContext>(connectionStr);

builder.Services.AddScoped<IRequestHandler<GetPurchaseStatistic, PurchaseStatistic>, GetPurchaseStatisticHandler>();

builder.Services.AddScoped<IRequestHandler<GetProducts, ProductModel[]>, GetProductsHandler>();
builder.Services.AddScoped<IRequestHandler<AddProduct, Unit>, AddProductHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateProduct, Unit>, UpdateProductHandler>();

builder.Services.AddScoped<IRequestHandler<GetProductKinds, ProductKindModel[]>, GetProductKindsHandler>();
builder.Services.AddScoped<IRequestHandler<AddProductKind, Unit>, AddProductKindHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateProductKind, Unit>, UpdateProductKindHandler>();

builder.Services.AddScoped<IRequestHandler<GetReceipts, ReceiptModel[]>, GetReceiptsHandler>();
builder.Services.AddScoped<IRequestHandler<AddReceipt, Unit>, AddReceiptHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateReceipt, Unit>, UpdateReceiptHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateReceiptTotal, Unit>, UpdateReceiptTotalHandler>();

builder.Services.AddScoped<IRequestHandler<GetReceiptItems, ReceiptItemModel[]>, GetReceiptItemsHandler>();
builder.Services.AddScoped<IRequestHandler<AddReceiptItem, Unit>, AddReceiptItemHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateReceiptItem, Unit>, UpdateReceiptItemHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteReceiptItem, Unit>, DeleteReceiptItemHandler>();

builder.Services.AddTransient<ServiceFactory>(p => p.GetService);
builder.Services.AddTransient<IMediator, Mediator>();

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
