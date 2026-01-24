using AwesomeAssertions;
using AwesomeAssertions.Execution;
using Shopping.Shared.Models.Common;
using System.Diagnostics.CodeAnalysis;

namespace Shopping.Tests.TestingUtilities
{
    public class EitherAssertion
    {
        private Either<Fail, Success> _actualValue;
        private AssertionChain _assertionChain;

        public EitherAssertion(Either<Fail, Success> actualValue, AssertionChain assertionChain)
        {
            _actualValue = actualValue;
            _assertionChain = assertionChain;
        }

        public AndConstraint<Either<Fail, Success>> BeSuccess([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        {
            _assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(_actualValue.IsRight && ((Success)_actualValue).Equals(Success.Instance))
                .FailWith("Expected to be Right but it is not");

            return new AndConstraint<Either<Fail, Success>>(_actualValue);
        }

        public AndWhichConstraint<Either<Fail, Success>, Fail> BeFail([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        {
            _assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!_actualValue.IsRight)
                .FailWith("Expected to be Right but it is not");

            return new AndWhichConstraint<Either<Fail, Success>, Fail>(_actualValue, (Fail)_actualValue);
        }
    }
}