using Shopping.Services.Interfaces;
using System.Threading.Channels;

namespace Shopping.Services.Common
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private readonly Channel<Func<IServiceProvider, CancellationToken, Task>> _channel = Channel.CreateUnbounded<Func<IServiceProvider, CancellationToken, Task>>();

        public void AddTask(Func<IServiceProvider, CancellationToken, Task> taskProvider)
        {
            _channel.Writer.WriteAsync(taskProvider);
        }

        public async Task<Func<IServiceProvider, CancellationToken, Task>> GetNextTaskProvider()
        {
            return await _channel.Reader.ReadAsync();
        }
    }
}