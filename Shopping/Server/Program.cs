using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Shopping.DataAccess;
using Shopping.SeriGraylog;
using Shopping.Server;
using Shopping.Server.Extensions;
using Shopping.Services;
using Microsoft.AspNetCore.Mvc;
using Shopping.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSeriGraylog();

var settings = builder.Configuration.Get<AppSettigns>();
builder.Services.AddSingleton<AppSettigns>(settings);

var connectionStr = builder.Configuration.GetConnectionString("Shopping");
builder.Services.AddSqlServer<ShoppingDbContext>(connectionStr);

builder.Services.RegisterServices();
builder.Services.RegisterMediatR();
builder.Services.RegisterMediatrServices();
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddMvcCore().AddMetricsCore(); // this is required to measure per request
builder.Host.UseMetrics(options =>
{
    options.EndpointOptions = endpointsOptions =>
    {
        endpointsOptions.EnvironmentInfoEndpointEnabled = false;
        endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
        endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
    };
});

builder.Services.AddHostedService<EfEventsCollectorHostedService>();
builder.Services.AddHostedService<BackgroundTaskProcessor>();

var app = builder.Build();
app.UseMetricsAllMiddleware();
app.UseGlobalExceptionHandling();

app.UseRouting();
app.UseCors(conf => conf.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();
app.Run();

public partial class Program
{ }