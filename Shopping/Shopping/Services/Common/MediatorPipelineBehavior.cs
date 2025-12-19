using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Shopping.Metrics;
using Shopping.Shared.Models.Common;
using System.Runtime.InteropServices;

namespace Shopping.Services.Common
{
    public sealed class MediatorPipelineBehavior<TRequest, TSuccessResponse> : IPipelineBehavior<TRequest, Either<Fail, TSuccessResponse>>
        where TRequest : IRequest<Either<Fail, TSuccessResponse>>
    {
        private readonly IValidator<TRequest> _validator;

        public MediatorPipelineBehavior([Optional] IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<Either<Fail, TSuccessResponse>> Handle(TRequest request, RequestHandlerDelegate<Either<Fail, TSuccessResponse>> next, CancellationToken cancellationToken)
        {
            if (_validator != null)
            {
                ValidationResult validationResult;
                using (ShoppingTelemetry.StartValidator<TRequest>())
                {
                    validationResult = await _validator.ValidateAsync(request, cancellationToken);
                }

                if (!validationResult.IsValid)
                {
                    return new Fail(FailType.Validation, validationResult.Errors.Select(err => err.ErrorMessage));
                }
            }

            using (ShoppingTelemetry.StartHandler<TRequest>())
            {
                return await next();
            }
        }
    }
}