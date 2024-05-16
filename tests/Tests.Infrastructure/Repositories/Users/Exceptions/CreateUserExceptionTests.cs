using Application.Common;
using Application.Extensions;

using Infrastructure.Repositories.Users.Exceptions;

namespace Tests.Infrastructure.Repositories.Users.Exceptions;

public class CreateUserExceptionTests
{
    private const string ANY_MESSAGE = "Could not create user.";

    [Fact]
    public void WhenErrorObject_ThenErrorMessageShouldBeSpecifiedMessage()
    {
        // Arrange
        CreateUserException createUserException = new(ANY_MESSAGE);

        // Act
        Error actual = createUserException.ErrorObject();

        // Assert
        actual.ErrorMessage.ShouldBe(ANY_MESSAGE);
    }

    [Fact]
    public void WhenErrorObject_ThenErrorTypeShouldBeCreateUserException()
    {
        // Arrange
        CreateUserException createUserException = new(ANY_MESSAGE);

        // Act
        Error actual = createUserException.ErrorObject();

        // Assert
        actual.ErrorType.ShouldBe("CreateUserException");
    }
}