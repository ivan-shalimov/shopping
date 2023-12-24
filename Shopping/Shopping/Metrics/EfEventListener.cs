using App.Metrics;
using System.Diagnostics.Tracing;

namespace Shopping.Metrics
{
    public sealed class EfEventListener : EventListener
    {
        private readonly IMetrics _metrics;

        public EfEventListener(IMetrics metrics)
        {
            _metrics = metrics;
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource.Name.Equals("Microsoft.EntityFrameworkCore"))
            {
                var args = new Dictionary<string, string>
                {
                    ["EventCounterIntervalSec"] = "3"
                };

                EnableEvents(eventSource, EventLevel.Verbose, EventKeywords.All, args);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData.EventName is null || !eventData.EventName.Equals("EventCounters") || eventData.Payload == null)
            {
                return;
            }

            foreach (var payload in eventData.Payload)
            {
                if (payload is IDictionary<string, object> eventPayload)
                {
                    ProcessEventPayload(eventPayload);
                }
            }
        }

        private void ProcessEventPayload(IDictionary<string, object> eventPayload)
        {
            if (!eventPayload.TryGetValue("Name", out var counterName))
            {
                return;
            }

            if ((string)counterName == "active-db-contexts")
            {
                if (!(eventPayload.TryGetValue("Mean", out var meanObj) && meanObj is double mean)
                || !(eventPayload.TryGetValue("Count", out var countObj) && countObj is int count))
                {
                    return;
                }

                var val = mean == 0 && count > 0 ? count : mean;
                val = double.IsNaN(val) ? 0 : val;

                _metrics.Measure.Gauge.SetValue(DatabaseMetrics.ActiveDbContextsGauge, val);
            }
        }
    }
}