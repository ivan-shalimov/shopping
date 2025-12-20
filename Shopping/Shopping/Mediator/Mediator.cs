using Microsoft.Extensions.DependencyInjection;
using Shopping.Metrics;
using Shopping.Shared.Models.Common;

namespace Shopping.Mediator
{
    internal class Mediator : IMediator
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Mediator(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

        public async Task<TResult> ExecuteAndReceiveWithoutValidation<TRequest, TResult>(TRequest request) where TRequest : IRequest<TResult>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var requestType = request.GetType().Name;
            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();
            using (ShoppingTelemetry.StartHandler(requestType))
            {
                // todo fix cancellation
                return await handler.Handle(request, CancellationToken.None);
            }
        }

        public async Task<Either<Fail, Success>> Execute<TRequest>(TRequest request) where TRequest : IRequest<Either<Fail, Success>>
        {
            return await ExecuteAndReceive<TRequest, Success>(request);
        }

        public async Task<Either<Fail, TSuccessResult>> ExecuteAndReceive<TRequest, TSuccessResult>(TRequest request)
            where TRequest : IRequest<Either<Fail, TSuccessResult>>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var requestType = request.GetType().Name;

            var validator = scope.ServiceProvider.GetService<IRequestVaidator<TRequest>>();
            if (validator != null)
            {
                using (ShoppingTelemetry.StartValidator(requestType))
                {
                    // todo fix cancellation
                    var (isValid, fail) = await validator.Validate(request, CancellationToken.None);
                    if (!isValid)
                    {
                        return fail;
                    }
                }
            }

            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, Either<Fail, TSuccessResult>>>();
            using (ShoppingTelemetry.StartHandler(requestType))
            {
                // todo fix cancellation
                return await handler.Handle(request, CancellationToken.None);
            }
        }
    }
}