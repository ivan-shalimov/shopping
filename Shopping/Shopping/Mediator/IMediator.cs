using Shopping.Shared.Models.Common;

namespace Shopping.Mediator
{
    public interface IMediator
    {
        Task<Either<Fail, Success>> Execute<TRequest>(TRequest request) where TRequest: IRequest<Either<Fail, Success>>;

        Task<Either<Fail, TSuccessResult>> ExecuteAndReceive<TRequest, TSuccessResult>(TRequest request) where TRequest: IRequest<Either<Fail, TSuccessResult>>;
    }
}