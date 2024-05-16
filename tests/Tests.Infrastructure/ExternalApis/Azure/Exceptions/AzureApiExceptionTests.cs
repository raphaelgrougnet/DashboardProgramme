using Application.Common;
using Application.Extensions;

using Infrastructure.ExternalApis.Azure.Exceptions;

namespace Tests.Infrastructure.ExternalApis.Azure.Exceptions;

public class AzureApiExceptionTests
{
    private const string SOME_MESSAGE = "Could not upload file to azure blob.";

    [Fact]
    public void WhenErrorObject_ThenErrorMessageShouldBeSpecifiedMessage()
    {
        // Arrange
        AzureApiException azureApiException = new(SOME_MESSAGE);

        // Act
        Error actual = azureApiException.ErrorObject();

        // Arrange
        actual.ErrorMessage.ShouldBe(SOME_MESSAGE);
    }

    [Fact]
    public void WhenErrorObject_ThenErrorTypeShouldBeAzureApiException()
    {
        // Arrange
        AzureApiException azureApiException = new(SOME_MESSAGE);

        // Act
        Error actual = azureApiException.ErrorObject();

        // Arrange
        actual.ErrorType.ShouldBe("AzureApiException");
    }
}