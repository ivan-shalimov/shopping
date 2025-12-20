namespace Shopping.Mediator
{
    public interface IRequest
    {
    }

    public interface IRequest<TResult> : IRequest
    {
    }
}