using Shopping.Mediator;
using Shopping.Services.Interfaces;

namespace Shopping.Server.Services
{
    public class BackgroundRequestProcessor : IHostedService, IDisposable
    {
        private const int MaxParallelTask = 1;
        private IServiceProvider _serviceProvider;
        private bool disposedValue;
        private readonly ILogger<BackgroundRequestProcessor> _logger;
        private IBackgroundRequestHandler _backgroundRequestHandler;
        private Task[] _parallelTasks;
        private CancellationTokenSource _stoppingCancellationTokenSource;

        public BackgroundRequestProcessor(
            IBackgroundRequestHandler backgroundRequestHandler,
            IServiceProvider serviceProvider,
            ILogger<BackgroundRequestProcessor> logger)
        {
            _logger = logger;
            _backgroundRequestHandler = backgroundRequestHandler;
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
                var request = await _backgroundRequestHandler.GetNextRequest();

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetService<IMediator>();
                        await mediator.Execute(request).ConfigureAwait(false);
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