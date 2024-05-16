using Application.Services.Notifications.Models;

using AutoMapper;

using Infrastructure.Mailing;
using Infrastructure.Mailing.Mapping;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using SendGrid.Helpers.Mail;

using Tests.Common.Mapping;

namespace Tests.Infrastructure.Mailing;

public class SendGridMessageFactoryTests
{
    private const string ANY_LINK = "www.google.com";
    private const string ANY_EMAIL = "garneau@spektrummedia.com";
    private const string ANY_LOCALE = "fr";
    private const string ANY_NAME = "Dashboard Programme";

    private readonly SendGridMessageFactory _sendGridMessageFactory;

    private readonly Mock<IWebHostEnvironment> _webHostEnvironment;

    public SendGridMessageFactoryTests()
    {
        _webHostEnvironment = new Mock<IWebHostEnvironment>();
        MailingSettings mailingSettings = new()
        {
            FromAddress = ANY_EMAIL, FromName = ANY_NAME, ToAddressForDevelopment = ANY_EMAIL
        };
        Mock<IOptions<MailingSettings>> mailingSettingsOptions = new();
        mailingSettingsOptions.Setup(x => x.Value).Returns(mailingSettings);

        Mapper mapper = new MapperBuilder().WithProfile<MailingMappingProfile>().Build();
        _sendGridMessageFactory =
            new SendGridMessageFactory(_webHostEnvironment.Object, mailingSettingsOptions.Object, mapper);
    }

    [Fact]
    public void WhenCreateFromModel_ThenReturnSendGridMessage()
    {
        // Arrange
        _webHostEnvironment.Setup(x => x.EnvironmentName).Returns(Environments.Staging);
        ForgotPasswordNotificationModel model = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);

        // Act
        SendGridMessage msg = _sendGridMessageFactory.CreateFromModel(model);

        // Assert
        msg.ShouldNotBeNull();
    }

    [Fact]
    public void WhenCreateFromModel_ThenUseMailingSettingsFromAddressAsFromEmail()
    {
        // Arrange
        ForgotPasswordNotificationModel model = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);

        // Act
        SendGridMessage msg = _sendGridMessageFactory.CreateFromModel(model);

        // Assert
        msg.From.Email.ShouldBe(ANY_EMAIL);
    }

    [Fact]
    public void WhenCreateFromModel_ThenUseMailingSettingsFromNameAsFromName()
    {
        // Arrange
        ForgotPasswordNotificationModel model = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);

        // Act
        SendGridMessage msg = _sendGridMessageFactory.CreateFromModel(model);

        // Assert
        msg.From.Name.ShouldBe(ANY_NAME);
    }

    [Fact]
    public void WhenCreateFromModel_ThenUseModelTemplateIdAsTemplateId()
    {
        // Arrange
        ForgotPasswordNotificationModel model = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);

        // Act
        SendGridMessage msg = _sendGridMessageFactory.CreateFromModel(model);

        // Assert
        msg.TemplateId.ShouldBe(model.TemplateId());
    }
}