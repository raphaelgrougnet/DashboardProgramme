using Application.Common;
using Application.Extensions;
using Application.Services.Users.Exceptions;

namespace Tests.Application.Services.Users.Exceptions;

public class ChangeAuthenticatedUserPasswordExceptionTests
{
    private const string ANY_MESSAGE = "Could not change authenticated user password.";

    [Fact]
    public void WhenErrorObject_ThenErrorMessageShouldBeSpecifiedMessage()
    {
        // Arrange
        ChangeAuthenticatedUserPasswordException changeAuthenticatedUserPasswordException = new(ANY_MESSAGE);

        // Act
        Error actual = changeAuthenticatedUserPasswordException.ErrorObject();

        // Assert
        actual.ErrorMessage.ShouldBe(ANY_MESSAGE);
    }

    [Fact]
    public void WhenErrorObject_ThenErrorTypeShouldBeChangeAuthenticatedUserPasswordException()
    {
        // Arrange
        ChangeAuthenticatedUserPasswordException changeAuthenticatedUserPasswordException = new(ANY_MESSAGE);

        // Act
        Error actual = changeAuthenticatedUserPasswordException.ErrorObject();

        // Assert
        actual.ErrorType.ShouldBe("ChangeAuthenticatedUserPasswordException");
    }
}