using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Services.Interfaces
{
    public interface IBackgroundMediatorRequestHandler
    {
        int QueueLength { get; }

        public ValueTask ExecuteInBackground<TRequest>(TRequest request) where TRequest : IRequest<Either<Fail, Success>>;

        public ValueTask<Func<IServiceProvider, Task>> GetNextTaskProvider();
    }
}