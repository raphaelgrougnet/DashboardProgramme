using Application.Common;
using Application.Extensions;
using Application.Services.Users.Exceptions;

namespace Tests.Application.Services.Users.Exceptions;

public class ChangeAuthenticatedUserPhoneNumberExceptionTests
{
    private const string ANY_MESSAGE = "Could not change authenticated user phone number.";

    [Fact]
    public void WhenErrorObject_ThenErrorMessageShouldBeSpecifiedMessage()
    {
        // Arrange
        ChangeAuthenticatedPhoneNumberException changeAuthenticatedPhoneNumberException = new(ANY_MESSAGE);

        // Act
        Error actual = changeAuthenticatedPhoneNumberException.ErrorObject();

        // Assert
        actual.ErrorMessage.ShouldBe(ANY_MESSAGE);
    }

    [Fact]
    public void WhenErrorObject_ThenErrorTypeShouldBeChangeAuthenticatedPhoneNumberException()
    {
        // Arrange
        ChangeAuthenticatedPhoneNumberException changeAuthenticatedPhoneNumberException = new(ANY_MESSAGE);

        // Act
        Error actual = changeAuthenticatedPhoneNumberException.ErrorObject();

        // Assert
        actual.ErrorType.ShouldBe("ChangeAuthenticatedPhoneNumberException");
    }
}