using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace Shopping.SeriGraylog
{
    public static class HostBuilderExtensions
    {
        public static void UseSeriLoki(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((hostContext, services, configuration) =>
            {
                var loggerSection = hostContext.Configuration.GetSection(nameof(LoggerSettings));
                var loggerSettings = loggerSection.Get<LoggerSettings>();
                if (loggerSettings == null)
                {
                    return;
                }

                var minimumLevel = Enum.TryParse<LogEventLevel>(loggerSettings.MinimumLevel, out var level) ? level : LogEventLevel.Warning;
                configuration.MinimumLevel.Is(minimumLevel);

                configuration.WriteTo.Console();

                configuration.WriteTo.GrafanaLoki(uri: loggerSettings.LokiUri, tenant: "k3s");

                configuration.Enrich.WithMachineName();
                configuration.Enrich.WithEnvironmentName();
                configuration.Enrich.FromLogContext();
            });
        }
    }
}