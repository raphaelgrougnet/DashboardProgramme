using Infrastructure.Mailing;

namespace Tests.Infrastructure.Mailing;

public class SendGridSettingsTests
{
    [Fact]
    public void GivenAnyApiKey_WhenNew_ThenApiKeyPropertyHasSameValue()
    {
        // Arrange
        const string API_KEY = "anyApiKey";

        // Act
        SendGridSettings sendGridSettings = new() { ApiKey = API_KEY };

        // Assert
        sendGridSettings.ApiKey.ShouldBe(API_KEY);
    }
}