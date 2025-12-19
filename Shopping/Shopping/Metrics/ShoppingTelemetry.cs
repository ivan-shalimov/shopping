using System.Diagnostics;

namespace Shopping.Metrics
{
    public sealed class ShoppingTelemetry
    {
        public static readonly ActivitySource ActivitySource = new ActivitySource(nameof(Shopping));

        public static Activity StartHandler<TRequest>()
        {
            var activity = ActivitySource.StartActivity("handler");
            activity?.SetTag(nameof(TRequest), typeof(TRequest).Name);
            return activity;
        }

        public static Activity StartValidator<TRequest>()
        {
            var activity = ActivitySource.StartActivity("handler");
            activity?.SetTag(nameof(TRequest), typeof(TRequest).Name);
            return activity;
        }
    }
}