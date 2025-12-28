using Shopping.Mediator;
using Shopping.Services.Interfaces;
using Shopping.Shared.Models.Common;
using System.Threading.Channels;

namespace Shopping.Services.Common
{
    public class BackgroundRequestHandler : IBackgroundRequestHandler
    {
        private readonly Channel<IRequest<Either<Fail, Success>>> _channel = Channel.CreateUnbounded<IRequest<Either<Fail, Success>>>();

        public void ExecuteInBackground(IRequest<Either<Fail, Success>> request)
        {
            _channel.Writer.WriteAsync(request);
        }

        public ValueTask<IRequest<Either<Fail, Success>>> GetNextRequest()
        {
            return _channel.Reader.ReadAsync();
        }
    }
}