using Application.Common;
using Application.Extensions;

using Infrastructure.Repositories.Users.Exceptions;

namespace Tests.Infrastructure.Repositories.Users.Exceptions;

public class UpdateUserExceptionTests
{
    private const string ANY_MESSAGE = "Could not update user.";

    [Fact]
    public void WhenErrorObject_ThenErrorMessageShouldBeSpecifiedMessage()
    {
        // Arrange
        UpdateUserException updateUserException = new(ANY_MESSAGE);

        // Act
        Error actual = updateUserException.ErrorObject();

        // Assert
        actual.ErrorMessage.ShouldBe(ANY_MESSAGE);
    }

    [Fact]
    public void WhenErrorObject_ThenErrorTypeShouldBeUpdateUserException()
    {
        // Arrange
        UpdateUserException updateUserException = new(ANY_MESSAGE);

        // Act
        Error actual = updateUserException.ErrorObject();

        // Assert
        actual.ErrorType.ShouldBe("UpdateUserException");
    }
}