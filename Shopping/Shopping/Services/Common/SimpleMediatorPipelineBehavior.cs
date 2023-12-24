using App.Metrics;
using MediatR;
using Shopping.Metrics;

namespace Shopping.Services.Common
{
    public class SimpleMediatorPipelineBehavior<TRequest, TSuccessResponse> : IPipelineBehavior<TRequest, TSuccessResponse>
        where TRequest : IRequest<TSuccessResponse>
    {
        protected IMetrics Metrics {  get; }

        public SimpleMediatorPipelineBehavior(IMetrics metrics)
        {
            Metrics = metrics;
        }

        public virtual async Task<TSuccessResponse> Handle(TRequest request, RequestHandlerDelegate<TSuccessResponse> next, CancellationToken cancellationToken)
        {
            using (Metrics.Measure.Timer.Time(ServiceMetrics.Handler, new MetricTags("Request", typeof(TRequest).Name)))
            {
                return await next();
            }
        }
    }
}