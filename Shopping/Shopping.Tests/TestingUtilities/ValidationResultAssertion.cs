using AwesomeAssertions;
using AwesomeAssertions.Execution;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace Shopping.Tests.TestingUtilities
{
    public class ValidationResultAssertion
    {
        private ValidationResult _actualValue;
        private AssertionChain _assertionChain;

        public ValidationResultAssertion(ValidationResult actualValue, AssertionChain assertionChain)
        {
            _actualValue = actualValue;
            _assertionChain = assertionChain;
        }

        public AndConstraint<ValidationResult> BeValid([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        {
            _assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(_actualValue.IsValid)
                .FailWith("Expected to be Valid but it is Not Valid");

            return new AndConstraint<ValidationResult>(_actualValue);
        }

        public AndWhichConstraint<ValidationResult, ValidationResult> BeNotValid([StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        {
            _assertionChain
                .BecauseOf(because, becauseArgs)
                .ForCondition(!_actualValue.IsValid)
                .FailWith("Expected to be Not Valid but it is Valid");

            return new AndWhichConstraint<ValidationResult, ValidationResult>(_actualValue, _actualValue);
        }
    }
}