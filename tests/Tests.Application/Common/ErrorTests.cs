using Application.Common;

namespace Tests.Application.Common;

public class ErrorTests
{
    private const string ANY_ERROR_TYPE = "UpdateUserException";
    private const string ANY_ERROR_MESSAGE = "Could not update user.";

    [Fact]
    public void GivenErrorMessage_WhenNew_ThenErrorMessageIsSameAsGivenErrorMessage()
    {
        // Act
        Error error = new(ANY_ERROR_TYPE, ANY_ERROR_MESSAGE);

        // Assert
        error.ErrorMessage.ShouldBe(ANY_ERROR_MESSAGE);
    }

    [Fact]
    public void GivenErrorType_WhenNew_ThenErrorMessageIsSameAsGivenErrorType()
    {
        // Act
        Error error = new(ANY_ERROR_TYPE, ANY_ERROR_MESSAGE);

        // Assert
        error.ErrorType.ShouldBe(ANY_ERROR_TYPE);
    }

    [Fact]
    public void WhenNew_ThenErrorMessageIsDefault()
    {
        // Act
        Error error = new();

        // Assert
        error.ErrorMessage.ShouldBe(default);
    }

    [Fact]
    public void WhenNew_ThenErrorTypeIsDefault()
    {
        // Act
        Error error = new();

        // Assert
        error.ErrorType.ShouldBe(default);
    }
}