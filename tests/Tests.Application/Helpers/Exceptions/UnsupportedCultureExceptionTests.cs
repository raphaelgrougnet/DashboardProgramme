using Application.Common;
using Application.Extensions;
using Application.Helpers.Exceptions;

namespace Tests.Application.Helpers.Exceptions;

public class UnsupportedCultureExceptionTests
{
    private const string ANY_MESSAGE = "Culture is not supported.";

    [Fact]
    public void WhenErrorObject_ThenErrorMessageShouldBeSpecifiedMessage()
    {
        // Arrange
        UnsupportedCultureException unsupportedCultureException = new(ANY_MESSAGE);

        // Act
        Error actual = unsupportedCultureException.ErrorObject();

        // Assert
        actual.ErrorMessage.ShouldBe(ANY_MESSAGE);
    }

    [Fact]
    public void WhenErrorObject_ThenErrorTypeShouldBeUnsupportedCultureException()
    {
        // Arrange
        UnsupportedCultureException unsupportedCultureException = new(ANY_MESSAGE);

        // Act
        Error actual = unsupportedCultureException.ErrorObject();

        // Assert
        actual.ErrorType.ShouldBe("UnsupportedCultureException");
    }
}