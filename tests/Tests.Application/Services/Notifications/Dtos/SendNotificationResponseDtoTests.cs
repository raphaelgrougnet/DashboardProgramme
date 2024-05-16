using Application.Services.Notifications.Dtos;

namespace Tests.Application.Services.Notifications.Dtos;

public class SendNotificationResponseDtoTests
{
    private const bool ANY_BOOLEAN = true;
    private const string ANY_ERROR_MESSAGE = "Error message";
    private readonly List<string> _anyErrorList = [ANY_ERROR_MESSAGE];

    [Fact]
    public void GivenAnyBoolean_WhenNewSendNotificationResponseDto_ThenSuccessfulPropertyShouldBeSameAsGiven()
    {
        // Arrange & act
        SendNotificationResponseDto sendNotificationResponseDto = new(ANY_BOOLEAN, _anyErrorList);

        // Assert
        sendNotificationResponseDto.Successful.ShouldBe(ANY_BOOLEAN);
    }

    [Fact]
    public void GivenAnyErrorList_WhenNewSendNotificationResponseDto_ThenErrorMessageFromListShouldBeInDtoErrorList()
    {
        // Arrange & act
        SendNotificationResponseDto sendNotificationResponseDto = new(ANY_BOOLEAN, _anyErrorList);

        // Assert
        sendNotificationResponseDto.ErrorMessages.ShouldContain(ANY_ERROR_MESSAGE);
    }
}