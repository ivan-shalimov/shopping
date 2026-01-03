using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Shopping.Telemetry
{
    public sealed class ShoppingTelemetry
    {
        public const string SourceName = nameof(Shopping);

        private static readonly ActivitySource ActivitySource = new ActivitySource(SourceName);
        private static readonly Meter Meter = new Meter(SourceName);

        private static readonly InstrumentAdvice<double> InstrumentAdvice =
            new InstrumentAdvice<double> { HistogramBucketBoundaries = [0.01, 0.05, 0.1, 0.5, 1, 5, 15, 30] };

        private static readonly Histogram<double> RequestValidationDurationHistogram =
            Meter.CreateHistogram(
            "shopping_request_validation_time",
            "s",
            "Histogram of request validation times in seconds",
            advice: InstrumentAdvice);

        private static readonly Histogram<double> RequestHandlingDurationHistogram =
            Meter.CreateHistogram(
            "shopping_request_handling_time",
            "s",
            "Histogram of request handling times in seconds",
            advice: InstrumentAdvice);

        private static readonly Counter<long> ExceptionsCounter =
            Meter.CreateCounter<long>(
            "shopping_exceptions_total",
            description: "Total number of exceptions");

        private static ObservableGauge<int> BackgroundQueueLengthGauge;

        public static IDisposable StartValidation(string requestType) => new MeasuredRequestOperation($"validation {requestType}", requestType, RequestValidationDurationHistogram);

        public static IDisposable StartHandling(string requestType) => new MeasuredRequestOperation($"handling {requestType}", requestType, RequestHandlingDurationHistogram);

        public static void TrackException(string exceptionFullName, Guid? exceptionId = null)
        {
            if (exceptionId == null)
            {
                ExceptionsCounter.Add(1, KeyValuePair.Create<string, object>("exception", exceptionFullName));
            }
            else
            {
                ExceptionsCounter.Add(
                    1,
                    KeyValuePair.Create<string, object>("exception", exceptionFullName),
                    KeyValuePair.Create<string, object>("exceptionId", exceptionId.ToString()));
            }
        }

        public static void RegisterBackgroundQueueMonitoring(Func<int> queueLengthProvider)
        {
            if (BackgroundQueueLengthGauge != null)
            {
                throw new InvalidOperationException("Background queue monitoring is already registered.");
            }

            BackgroundQueueLengthGauge = Meter.CreateObservableGauge(
                "shopping_background_queue_length",
                queueLengthProvider,
                description: "Current length of the background processing queue");
        }

        private class MeasuredRequestOperation : IDisposable
        {
            private readonly Histogram<double> _histogram;
            private readonly KeyValuePair<string, object> _histogramTag;
            private readonly Activity _activity;
            private Stopwatch _stopWatch;

            public MeasuredRequestOperation(string operationName, string requestType, Histogram<double> histogram)
            {
                _histogram = histogram;
                _histogramTag = KeyValuePair.Create<string, object>(nameof(requestType), requestType);

                _activity = ActivitySource.StartActivity(operationName, kind: ActivityKind.Internal);
                _activity?.SetTag(nameof(requestType), requestType);
                _stopWatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                _stopWatch.Stop();
                _histogram.Record(_stopWatch.Elapsed.TotalSeconds, _histogramTag);
                _activity?.Dispose();
            }
        }
    }
}