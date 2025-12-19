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
            if (string.IsNullOrWhiteSpace(otplCollectorEndpoint))
            {
                // if endpoint is not set, configuration can be done
                return builder;
            }

            var otplCollectorEndpointUri = new Uri(otplCollectorEndpoint);
            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(serviceName: "Shopping.Server"));
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
                logging.AddOtlpExporter(opts => opts.Endpoint = otplCollectorEndpointUri);
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
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter(opts => opts.Endpoint = otplCollectorEndpointUri);
                })
                .WithMetrics(metricsProviderBuilder =>
                {
                    metricsProviderBuilder
                        .SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(serviceName: "Shopping.Server"))
                         .AddAspNetCoreInstrumentation()
                         .AddRuntimeInstrumentation()
                         .AddHttpClientInstrumentation()
                        .AddOtlpExporter((opts, metricReaderOptions) =>
                        {
                            opts.Endpoint = otplCollectorEndpointUri;
                            metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 15000;

                        });
                });

            return builder;
        }
    }
}