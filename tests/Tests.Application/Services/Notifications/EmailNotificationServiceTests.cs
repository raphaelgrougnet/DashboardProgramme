using Application.Interfaces.Mailing;
using Application.Services.Notifications;
using Application.Services.Notifications.Models;

using Domain.Entities.Identity;

using Tests.Application.TestCollections;
using Tests.Common.Builders;

namespace Tests.Application.Services.Notifications;

[Collection(ApplicationTestCollection.NAME)]
public class EmailNotificationServiceTests
{
    private const string ANY_EMAIL = "garneau@spektrummedia.com";
    private const string ANY_LINK = "www.google.com";
    private const string ANY_FIRST_NAME = "John";
    private const string ANY_CODE = "123456";

    private readonly EmailNotificationService _emailNotificationService;

    private readonly Mock<IEmailSender> _emailSender;

    private readonly UserBuilder _userBuilder;

    public EmailNotificationServiceTests()
    {
        _userBuilder = new UserBuilder();

        _emailSender = new Mock<IEmailSender>();
        _emailNotificationService = new EmailNotificationService(_emailSender.Object);
    }

    [Fact]
    public async Task
        GivenAnyEmailAndCode_WhenSendTwoFactorAuthenticationCode_ThenSendTwoFactorAuthenticationNotification()
    {
        // Act
        await _emailNotificationService.SendTwoFactorAuthenticationCodeNotification(ANY_EMAIL, ANY_CODE);

        // Assert
        _emailSender.Verify(x => x.SendAsync(It.IsAny<TwoFactorAuthenticationNotificationModel>()));
    }

    [Fact]
    public async Task GivenAnyUserAndLink_WhenSendForgotPasswordNotification_ThenSendForgotPasswordEmail()
    {
        // Arrange
        User user = _userBuilder.WithEmail(ANY_EMAIL).Build();

        // Act
        await _emailNotificationService.SendForgotPasswordNotification(user, ANY_LINK);

        // Assert
        _emailSender.Verify(x => x.SendAsync(It.IsAny<ForgotPasswordNotificationModel>()));
    }
}