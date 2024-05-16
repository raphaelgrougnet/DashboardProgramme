using Application.Common;
using Application.Extensions;
using Application.Services.Users.Exceptions;

namespace Tests.Application.Services.Users.Exceptions;

public class ChangeAuthenticatedUserEmailExceptionTests
{
    private const string ANY_MESSAGE = "Could not change authenticated user email.";

    [Fact]
    public void WhenErrorObject_ThenErrorMessageShouldBeSpecifiedMessage()
    {
        // Arrange
        ChangeAuthenticatedUserEmailException changeAuthenticatedUserEmailException = new(ANY_MESSAGE);

        // Act
        Error actual = changeAuthenticatedUserEmailException.ErrorObject();

        // Assert
        actual.ErrorMessage.ShouldBe(ANY_MESSAGE);
    }

    [Fact]
    public void WhenErrorObject_ThenErrorTypeShouldBeChangeAuthenticatedUserEmailException()
    {
        // Arrange
        ChangeAuthenticatedUserEmailException changeAuthenticatedUserEmailException = new(ANY_MESSAGE);

        // Act
        Error actual = changeAuthenticatedUserEmailException.ErrorObject();

        // Assert
        actual.ErrorType.ShouldBe("ChangeAuthenticatedUserEmailException");
    }
}