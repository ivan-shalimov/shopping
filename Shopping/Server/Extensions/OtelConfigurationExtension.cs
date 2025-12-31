using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shopping.Metrics;

namespace Shopping.Server.Extensions
{
    public static class OtelConfigurationExtension
    {
        public static IHostApplicationBuilder AddOpenTelemetry(this IHostApplicationBuilder builder, string otplCollectorEndpoint)
        {
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            if (string.IsNullOrWhiteSpace(otplCollectorEndpoint) && !isDevelopment)
            {
                // if endpoint is not set, configuration can be done
                Console.WriteLine($"[Starting service]: OTPL configuration skipped otplCollectorEndpointIsEmpty:{string.IsNullOrWhiteSpace(otplCollectorEndpoint)}, isDevelopment:{isDevelopment}");
                return builder;
            }

            Console.WriteLine($"[Starting service]: OtplCollectorEndpoint {otplCollectorEndpoint}");

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(serviceName: "Shopping.Server"));
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
                if (isDevelopment)
                {
                    logging.AddConsoleExporter();
                }
                else
                {
                    var otplCollectorEndpointUri = new Uri(otplCollectorEndpoint);
                    logging.AddOtlpExporter(opts => opts.Endpoint = otplCollectorEndpointUri);
                }
            });

            builder.Services.AddOpenTelemetry()
                .WithTracing(tracerProviderBuilder =>
                {
                    tracerProviderBuilder
                        .SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(serviceName: "Shopping.Server"))
                        .AddSource(ShoppingTelemetry.ActivitySource.Name)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation();

                    if (isDevelopment)
                    {
                        tracerProviderBuilder.AddConsoleExporter();
                    }
                    else
                    {
                        var otplCollectorEndpointUri = new Uri(otplCollectorEndpoint);
                        tracerProviderBuilder.AddOtlpExporter(opts => opts.Endpoint = otplCollectorEndpointUri);
                    }
                })
                .WithMetrics(metricsProviderBuilder =>
                {
                    metricsProviderBuilder
                        .SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(serviceName: "Shopping.Server"))
                         .AddAspNetCoreInstrumentation()
                         .AddRuntimeInstrumentation()
                         .AddHttpClientInstrumentation();

                    if (isDevelopment)
                    {
                        metricsProviderBuilder.AddPrometheusExporter();
                    }
                    else
                    {
                        metricsProviderBuilder.AddOtlpExporter((opts, metricReaderOptions) =>
                        {
                            var otplCollectorEndpointUri = new Uri(otplCollectorEndpoint);
                            opts.Endpoint = otplCollectorEndpointUri;
                            metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 15000;
                        });
                    }
                });

            return builder;
        }
    }
}