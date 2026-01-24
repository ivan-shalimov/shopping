using AwesomeAssertions.Execution;
using FluentValidation.Results;
using Shopping.Shared.Models.Common;
using System.Diagnostics.CodeAnalysis;

namespace Shopping.Tests.TestingUtilities
{
    public static class AssertionExtensions
    {
        public static EitherAssertion Should([NotNull] this Either<Fail, Success> actualValue)
        {
            return new EitherAssertion(actualValue, AssertionChain.GetOrCreate());
        }

        public static ValidationResultAssertion Should([NotNull] this ValidationResult actualValue)
        {
            return new ValidationResultAssertion(actualValue, AssertionChain.GetOrCreate());
        }
    }
}