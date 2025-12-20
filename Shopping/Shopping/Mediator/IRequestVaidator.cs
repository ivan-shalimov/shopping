using Shopping.Shared.Models.Common;

namespace Shopping.Mediator
{
    public interface IRequestVaidator<TRequest> where TRequest : IRequest
    {
        Task<(bool isValid, Fail fail)> Validate(TRequest request, CancellationToken cancellationToken);
    }
}