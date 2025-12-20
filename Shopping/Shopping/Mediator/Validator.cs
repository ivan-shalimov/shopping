using FluentValidation;
using Shopping.Shared.Models.Common;

namespace Shopping.Mediator
{
    public class Validator<TRequest> : AbstractValidator<TRequest>, IRequestVaidator<TRequest> where TRequest : IRequest
    {
        public async Task<(bool isValid, Fail fail)> Validate(TRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return (false, new Fail(FailType.Validation, validationResult.Errors.Select(err => err.ErrorMessage)));
            }

            return (true, null);
        }
    }
}