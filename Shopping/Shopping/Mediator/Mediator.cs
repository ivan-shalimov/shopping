using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Metrics;
using Shopping.Shared.Models.Common;

namespace Shopping.Mediator
{
    internal class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public async Task<Either<Fail, Success>> Execute<TRequest>(TRequest request) where TRequest : IRequest<Either<Fail, Success>>
        {
            return await ExecuteAndReceive<TRequest, Success>(request);
        }

        public async Task<Either<Fail, TSuccessResult>> ExecuteAndReceive<TRequest, TSuccessResult>(TRequest request)
            where TRequest : IRequest<Either<Fail, TSuccessResult>>
        {
            var requestType = request.GetType().Name;

            var validator = _serviceProvider.GetService<AbstractValidator<TRequest>>();
            if (validator != null)
            {
                using (ShoppingTelemetry.StartValidator(requestType))
                {
                    // todo fix cancellation
                    var validationResult = await validator.ValidateAsync(request, CancellationToken.None);

                    if (!validationResult.IsValid)
                    {
                        return new Fail(FailType.Validation, validationResult.Errors.Select(err => err.ErrorMessage));
                    }
                }
            }

            var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, Either<Fail, TSuccessResult>>>();
            using (ShoppingTelemetry.StartHandler(requestType))
            {
                // todo fix cancellation
                return await handler.Handle(request, CancellationToken.None);
            }
        }
    }
}