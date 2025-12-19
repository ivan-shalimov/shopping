using MediatR;
using Shopping.Metrics;

namespace Shopping.Services.Common
{
    public class SimpleMediatorPipelineBehavior<TRequest, TSuccessResponse> : IPipelineBehavior<TRequest, TSuccessResponse>
        where TRequest : IRequest<TSuccessResponse>
    {
        public virtual async Task<TSuccessResponse> Handle(TRequest request, RequestHandlerDelegate<TSuccessResponse> next, CancellationToken cancellationToken)
        {
            using (ShoppingTelemetry.StartHandler<TRequest>())
            {
                return await next();
            }
        }
    }
}