using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shopping.Telemetry;

namespace Shopping.Server.Extensions
{
    public static class OtelConfigurationExtension
    {
        public static IHostApplicationBuilder AddOpenTelemetry(this IHostApplicationBuilder builder, string otplCollectorEndpoint)
        {
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            var hasOtplCollectorEndpoint = !string.IsNullOrWhiteSpace(otplCollectorEndpoint);
            var otplCollectorEndpointUri = hasOtplCollectorEndpoint ? new Uri(otplCollectorEndpoint) : null;

            if (!hasOtplCollectorEndpoint && !isDevelopment)
            {
                // if endpoint is not set, configuration can be done
                Console.WriteLine($"[Starting service]: OTPL configuration skipped hasOtplCollectorEndpoint:{hasOtplCollectorEndpoint}, isDevelopment:{isDevelopment}");
                return builder;
            }

            Console.WriteLine($"[Starting service]: OtplCollectorEndpoint {otplCollectorEndpoint}");

            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
                if (isDevelopment)
                {
                    logging.AddConsoleExporter();
                }

                if (hasOtplCollectorEndpoint)
                {
                    logging.AddOtlpExporter(opts => opts.Endpoint = otplCollectorEndpointUri);
                }
            });

            builder.Services.AddOpenTelemetry()
                .WithTracing(tracerProviderBuilder =>
                {
                    tracerProviderBuilder
                        .AddSource(ShoppingTelemetry.SourceName)
                        .AddAspNetCoreInstrumentation();

                    if (isDevelopment)
                    {
                        tracerProviderBuilder.AddConsoleExporter();
                    }

                    if (hasOtplCollectorEndpoint)
                    {
                        tracerProviderBuilder.AddOtlpExporter(opts => opts.Endpoint = otplCollectorEndpointUri);
                    }
                })
                .WithMetrics(metricsProviderBuilder =>
                {
                    metricsProviderBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddMeter(ShoppingTelemetry.SourceName);

                    if (isDevelopment)
                    {
                        metricsProviderBuilder.AddPrometheusExporter();
                    }

                    if (hasOtplCollectorEndpoint)
                    {
                        metricsProviderBuilder.AddOtlpExporter((opts, metricReaderOptions) =>
                        {
                            opts.Endpoint = otplCollectorEndpointUri;
                            metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 15000;
                        });
                    }
                })
                .ConfigureResource(config =>
                {
                    config.AddService("Shopping.Server");
                    config.AddAttributes(new Dictionary<string, object>
                    {
                        ["environment"] = isDevelopment ? "dev" : "prod"
                    });
                });

            return builder;
        }
    }
}