using Microsoft.Extensions.DependencyInjection;
using Shopping.Mediator;
using Shopping.Services.Interfaces;
using Shopping.Shared.Models.Common;
using System.Threading.Channels;

namespace Shopping.Services.Common
{
    public class BackgroundMediatorRequestHandler : IBackgroundMediatorRequestHandler
    {
        private readonly Channel<Func<IServiceProvider, Task>> _channel = Channel.CreateUnbounded<Func<IServiceProvider, Task>>();

        public int QueueLength => _channel.Reader.Count;

        public ValueTask ExecuteInBackground<TRequest>(TRequest request) where TRequest : IRequest<Either<Fail, Success>>
        {
            // it requires to capture the request type here because using only interface will cause issues with DI resolution
            return _channel.Writer.WriteAsync(async sp =>
             {
                 var mediator = sp.GetRequiredService<IMediator>();
                 await mediator.Execute(request);
             });
        }

        public ValueTask<Func<IServiceProvider, Task>> GetNextTaskProvider()
        {
            return _channel.Reader.ReadAsync();
        }
    }
}