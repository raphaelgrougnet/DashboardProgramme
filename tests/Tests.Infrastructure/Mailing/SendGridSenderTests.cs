using System.Net;
using System.Text;
using System.Text.Json;

using Application.Services.Notifications.Dtos;
using Application.Services.Notifications.Models;

using Infrastructure.Mailing;

using Microsoft.Extensions.Logging;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace Tests.Infrastructure.Mailing;

public class SendGridSenderTests
{
    private const string ANY_LINK = "www.google.com";
    private const string ANY_EMAIL = "garneau@spektrummedia.com";
    private const string ANY_LOCALE = "fr";
    private const string ANY_ERROR_MESSAGE = "Error message";

    private readonly Mock<ISendGridClient> _sendGridClient;

    private readonly SendGridSender _sendGridSender;

    public SendGridSenderTests()
    {
        Mock<ILogger<SendGridSender>> logger = new();
        _sendGridClient = new Mock<ISendGridClient>();
        _sendGridSender = new SendGridSender(logger.Object, _sendGridClient.Object,
            new Mock<ISendGridMessageFactory>().Object);
    }


    private HttpContent BuildSendGridResponseWithErrors(List<SendGridError> sendGridErrors)
    {
        Dictionary<string, dynamic> dictionary = new() { { "errors", sendGridErrors } };
        return new StringContent(JsonSerializer.Serialize(dictionary), Encoding.UTF8, "application/json");
    }

    [Fact]
    public async Task GivenSendingEmailFails_WhenSendAsync_ThenReturnListOfErrorMessages()
    {
        // Arrange
        ForgotPasswordNotificationModel model = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);
        List<SendGridError> errors = [new SendGridError { Message = ANY_ERROR_MESSAGE }];
        _sendGridClient
            .Setup(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response(HttpStatusCode.BadRequest, BuildSendGridResponseWithErrors(errors), default));

        // Act
        SendNotificationResponseDto responseDto = await _sendGridSender.SendAsync(model);

        // Assert
        responseDto.ErrorMessages.ShouldContain(ANY_ERROR_MESSAGE);
    }

    [Fact]
    public async Task GivenSendingEmailFails_WhenSendAsync_ThenReturnSuccessfulFalse()
    {
        // Arrange
        ForgotPasswordNotificationModel model = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);
        List<SendGridError> errors = [new SendGridError { Message = ANY_ERROR_MESSAGE }];
        _sendGridClient
            .Setup(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response(HttpStatusCode.BadRequest, BuildSendGridResponseWithErrors(errors), default));

        // Act
        SendNotificationResponseDto responseDto = await _sendGridSender.SendAsync(model);

        // Assert
        responseDto.Successful.ShouldBeFalse();
    }

    [Fact]
    public async Task GivenSendingEmailWorks_WhenSendAsync_ThenReturnEmptyErrorList()
    {
        // Arrange
        ForgotPasswordNotificationModel model = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);
        _sendGridClient
            .Setup(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response(HttpStatusCode.OK, default, default));

        // Act
        SendNotificationResponseDto responseDto = await _sendGridSender.SendAsync(model);

        // Assert
        responseDto.ErrorMessages.ShouldBeEmpty();
    }

    [Fact]
    public async Task GivenSendingEmailWorks_WhenSendAsync_ThenReturnSuccessfulTrue()
    {
        // Arrange
        ForgotPasswordNotificationModel model = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);
        _sendGridClient
            .Setup(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response(HttpStatusCode.OK, default, default));

        // Act
        SendNotificationResponseDto responseDto = await _sendGridSender.SendAsync(model);

        // Assert
        responseDto.Successful.ShouldBeTrue();
    }
}