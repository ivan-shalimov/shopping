using Shopping.Mediator;
using Shopping.Services.Interfaces;

namespace Shopping.Server.Services
{
    public class BackgroundTaskProcessor : IHostedService, IDisposable
    {
        private const int MaxParallelTask = 1;
        private IServiceProvider _serviceProvider;
        private bool disposedValue;
        private readonly ILogger<BackgroundTaskProcessor> _logger;
        private IBackgroundTaskManager _backgroundTaskManager;
        private Task[] _parallelTasks;
        private CancellationTokenSource _stoppingCancellationTokenSource;

        public BackgroundTaskProcessor(
            IBackgroundTaskManager backgroundTaskManager,
            IServiceProvider serviceProvider,
            ILogger<BackgroundTaskProcessor> logger)
        {
            _logger = logger;
            _backgroundTaskManager = backgroundTaskManager;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _stoppingCancellationTokenSource = new CancellationTokenSource();
            _parallelTasks = Enumerable.Range(0, MaxParallelTask)
               .Select(i => Handle(_stoppingCancellationTokenSource.Token))
               .ToArray();
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_parallelTasks == null)
            {
                return;
            }

            try
            {
                _stoppingCancellationTokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(Task.WhenAll(_parallelTasks), Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
            }

            _parallelTasks = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StopAsync(CancellationToken.None).GetAwaiter().GetResult();
                    _stoppingCancellationTokenSource.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private async Task Handle(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var taskProvider = await _backgroundTaskManager.GetNextTaskProvider();

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var mediatr = scope.ServiceProvider.GetService<IMediator>();
                        await taskProvider(scope.ServiceProvider, cancellationToken).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException) { } // Prevent throwing if the Delay is cancelled
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Background task was not handled successful.");
                }
            }
        }
    }
}