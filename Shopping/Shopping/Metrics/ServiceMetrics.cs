using App.Metrics;
using App.Metrics.Timer;

namespace Shopping.Metrics
{
    public static class ServiceMetrics
    {
        private static readonly string ContextName = "application_services";

        public static TimerOptions Handler = new TimerOptions
        {
            Context = ContextName,
            Name = "handler",
            MeasurementUnit = Unit.Calls,
            DurationUnit = TimeUnit.Microseconds,
            RateUnit = TimeUnit.Microseconds,
        }; 
        
        public static TimerOptions Validator = new TimerOptions
        {
            Context = ContextName,
            Name = "validator",
            MeasurementUnit = Unit.Calls,
            DurationUnit = TimeUnit.Microseconds,
            RateUnit = TimeUnit.Microseconds,
        };
    }
}