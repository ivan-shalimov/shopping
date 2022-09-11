using FluentValidation;
using MediatR;
using Shopping.Shared.Models.Common;

namespace Shopping.Services.Common
{
    public sealed class ValidationPipelineBehavior<TRequest, TSuccessResponse, TValidator> : IPipelineBehavior<TRequest, Either<Fail, TSuccessResponse>>
       where TValidator : class, IValidator<TRequest>
        where TRequest : IRequest<Either<Fail, TSuccessResponse>>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationPipelineBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<Either<Fail, TSuccessResponse>> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Either<Fail, TSuccessResponse>> next)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            return new Fail(FailType.Validation, validationResult.Errors.Select(err => err.ErrorMessage));
        }
    }
}