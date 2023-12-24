using App.Metrics;
using Shopping.Metrics;

namespace Shopping.Server.Services
{
    public class EfEventsCollectorHostedService : IHostedService, IDisposable
    {
        private readonly IMetrics _metrics;
        private EfEventListener _efEventListener;

        public EfEventsCollectorHostedService(IMetrics metrics)
        {
            _metrics = metrics;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _efEventListener = new EfEventListener(_metrics);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _efEventListener?.Dispose();
        }
    }
}