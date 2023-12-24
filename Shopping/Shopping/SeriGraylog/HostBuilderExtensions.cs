using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;

namespace Shopping.SeriGraylog
{
    public static class HostBuilderExtensions
    {
        public static void UseSeriGraylog(this IHostBuilder hostBuilder)
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

                var graylogSinkOptions = loggerSection.GetSection(nameof(GraylogSinkOptions)).Get<GraylogSinkOptions>();
                if (graylogSinkOptions != null)
                {
                    configuration.WriteTo.Graylog(graylogSinkOptions);
                }

                configuration.Enrich.WithMachineName();
                configuration.Enrich.WithEnvironmentName();
                configuration.Enrich.FromLogContext();
            });
        }
    }
}