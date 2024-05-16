using Application.Common;
using Application.Extensions;

using Infrastructure.Repositories.Users.Exceptions;

namespace Tests.Infrastructure.Repositories.Users.Exceptions;

public class RoleNotFoundExceptionTests
{
    private const string ANY_MESSAGE = "Could not find role with given name.";

    [Fact]
    public void WhenErrorObject_ThenErrorMessageShouldBeSpecifiedMessage()
    {
        // Arrange
        RoleNotFoundException roleNotFoundException = new(ANY_MESSAGE);

        // Act
        Error actual = roleNotFoundException.ErrorObject();

        // Assert
        actual.ErrorMessage.ShouldBe(ANY_MESSAGE);
    }

    [Fact]
    public void WhenErrorObject_ThenErrorTypeShouldBeRoleNotFoundException()
    {
        // Arrange
        RoleNotFoundException roleNotFoundException = new(ANY_MESSAGE);

        // Act
        Error actual = roleNotFoundException.ErrorObject();

        // Assert
        actual.ErrorType.ShouldBe("RoleNotFoundException");
    }
}