using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;

namespace Shopping.Metrics
{
    public static class DatabaseMetrics
    {
        private static readonly string ContextName = "application_database";

        public static CounterOptions ContextInstancesCounter = new CounterOptions
        {
            Context = ContextName,
            Name = "context_instances",
            MeasurementUnit = Unit.Items,
        };

        public static GaugeOptions ActiveDbContextsGauge = new GaugeOptions
        {
            Context = ContextName,
            Name = "active_db_contexts",
            MeasurementUnit = Unit.Bytes
        };
    }
}