using System.Diagnostics;

namespace Shopping.Metrics
{
    public sealed class ShoppingTelemetry
    {
        public static readonly ActivitySource ActivitySource = new ActivitySource(nameof(Shopping));

        public static Activity StartHandler(string requestType)
        {
            var activity = ActivitySource.StartActivity("handler");
            activity?.SetTag(nameof(requestType), requestType);
            return activity;
        }

        public static Activity StartValidator(string requestType)
        {
            var activity = ActivitySource.StartActivity("validator");
            activity?.SetTag(nameof(requestType), requestType);
            return activity;
        }
    }
}