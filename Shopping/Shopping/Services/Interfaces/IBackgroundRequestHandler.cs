using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Services.Interfaces
{
    public interface IBackgroundRequestHandler
    {
        public void ExecuteInBackground(IRequest<Either<Fail, Success>> request);

        public ValueTask<IRequest<Either<Fail, Success>>> GetNextRequest();
    }
}