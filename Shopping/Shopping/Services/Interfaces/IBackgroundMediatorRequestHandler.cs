using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Services.Interfaces
{
    public interface IBackgroundMediatorRequestHandler
    {
        public void ExecuteInBackground<TRequest>(TRequest request) where TRequest : IRequest<Either<Fail, Success>>;

        public ValueTask<Func<IServiceProvider, Task>> GetNextTaskProvider();
    }
}