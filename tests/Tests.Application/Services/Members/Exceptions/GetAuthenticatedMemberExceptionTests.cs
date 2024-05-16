using Application.Common;
using Application.Extensions;
using Application.Services.Members.Exceptions;

namespace Tests.Application.Services.Members.Exceptions;

public class GetAuthenticatedMemberExceptionTests
{
    private const string ANY_MESSAGE = "Could not get authenticated member.";

    [Fact]
    public void WhenErrorObject_ThenErrorMessageShouldBeSpecifiedMessage()
    {
        // Arrange
        GetAuthenticatedMemberException getAuthenticatedMemberException = new(ANY_MESSAGE);

        // Act
        Error actual = getAuthenticatedMemberException.ErrorObject();

        // Assert
        actual.ErrorMessage.ShouldBe(ANY_MESSAGE);
    }

    [Fact]
    public void WhenErrorObject_ThenErrorTypeShouldBeGetAuthenticatedMemberException()
    {
        // Arrange
        GetAuthenticatedMemberException getAuthenticatedMemberException = new(ANY_MESSAGE);

        // Act
        Error actual = getAuthenticatedMemberException.ErrorObject();

        // Assert
        actual.ErrorType.ShouldBe("GetAuthenticatedMemberException");
    }
}