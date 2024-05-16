using Application.Common;
using Application.Extensions;

using FastEndpoints;

using FluentValidation.Results;

using Web.Extensions;

namespace Tests.Web.Extensions;

public class ValidationFailureExceptionExtensionsTests
{
    private const string ANY_ERROR_CODE = "ErrorCode";
    private const string ANY_ERROR_MESSAGE = "Some error message";

    [Fact]
    public void GivenExceptionWithFailures_WhenErrorObjects_ThenReturnedErrorMessageShouldBeFailureErrorMessage()
    {
        // Arrange
        ValidationFailure failure = new() { ErrorCode = ANY_ERROR_CODE, ErrorMessage = ANY_ERROR_MESSAGE };
        ValidationFailureException exception = new(failure.IntoList(), ANY_ERROR_MESSAGE);

        // Act
        IEnumerable<Error> errors = exception.ErrorObjects();

        errors.ShouldContain(x => x.ErrorType == ANY_ERROR_CODE);
    }

    [Fact]
    public void GivenExceptionWithFailures_WhenErrorObjects_ThenReturnedErrorTypeShouldBeFailureErrorCode()
    {
        // Arrange
        ValidationFailure failure = new() { ErrorCode = ANY_ERROR_CODE, ErrorMessage = ANY_ERROR_MESSAGE };
        ValidationFailureException exception = new(failure.IntoList(), ANY_ERROR_MESSAGE);

        // Act
        IEnumerable<Error> errors = exception.ErrorObjects();

        errors.ShouldContain(x => x.ErrorType == ANY_ERROR_CODE);
    }

    [Fact]
    public void GivenExceptionWithFailures_WhenErrorObjects_ThenReturnListOfError()
    {
        // Arrange
        ValidationFailure failure = new() { ErrorCode = ANY_ERROR_CODE, ErrorMessage = ANY_ERROR_MESSAGE };
        ValidationFailureException exception = new(failure.IntoList(), ANY_ERROR_MESSAGE);

        // Act
        IEnumerable<Error> errors = exception.ErrorObjects();

        errors.ShouldNotBeEmpty();
    }

    [Fact]
    public void GivenExceptionWithNullListOfFailures_WhenErrorObjects_ThenReturnEmptyList()
    {
        // Arrange
        ValidationFailureException exception = new();

        // Act
        IEnumerable<Error> errors = exception.ErrorObjects();

        errors.ShouldBeEmpty();
    }
}