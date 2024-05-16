using Application.Services.Notifications.Models;

using Tests.Common.Extensions;

namespace Tests.Application.Services.Notifications.Models;

public class ForgotPasswordNotificationModelTests
{
    private const string ANY_LINK = "www.google.com";
    private const string ANY_EMAIL = "garneau@spektrummedia.com";
    private const string ANY_LOCALE = "fr";
    private const string EN_TEMPLATE_ID = "d-6bceb5f892064a7b95cc03fe16b45943";
    private const string FR_TEMPLATE_ID = "d-ccea0bf1594048259d41fb52c2c23614";

    [Fact]
    public void GivenAnyEmail_WhenNewForgotPasswordNotificationModel_ThenDestinationEmailShouldBeSameAsGivenEmail()
    {
        // Arrange & act
        ForgotPasswordNotificationModel forgotPasswordNotificationModel = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);

        // Assert
        forgotPasswordNotificationModel.Destination.ShouldBe(ANY_EMAIL);
    }


    [Fact]
    public void GivenAnyLink_WhenNewForgotPasswordNotificationModel_ThenLinkShouldBeSameAsGivenLink()
    {
        // Arrange & act
        ForgotPasswordNotificationModel forgotPasswordNotificationModel = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);

        // Assert
        forgotPasswordNotificationModel.Link.ShouldBe(ANY_LINK);
    }

    [Fact]
    public void GivenAnyLocale_WhenNewForgotPasswordNotificationModel_ThenLocaleShouldBeSameAsGivenLocale()
    {
        // Arrange & act
        ForgotPasswordNotificationModel forgotPasswordNotificationModel = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);

        // Assert
        forgotPasswordNotificationModel.Locale.ShouldBe(ANY_LOCALE);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void GivenNullEmptyOrWhitespaceEmail_WhenNewForgotPasswordNotificationModel_ThenThrowArgumentException(
        string email)
    {
        // Act & assert
        Assert.Throws<ArgumentException>(() => new ForgotPasswordNotificationModel(email, ANY_LOCALE, ANY_LINK));
    }

    [Fact]
    public void WhenLocaleIsEnglish_WhenTemplateId_ThenReturnEnglishTemplateId()
    {
        // Arrange
        ForgotPasswordNotificationModel forgotPasswordNotificationModel = new(ANY_EMAIL, "en", ANY_LINK);

        // Act
        string templateId = forgotPasswordNotificationModel.TemplateId();

        // Assert
        templateId.ShouldBe(EN_TEMPLATE_ID);
    }

    [Fact]
    public void WhenLocaleIsFrench_WhenTemplateId_ThenReturnFrenchTemplateId()
    {
        // Arrange
        ForgotPasswordNotificationModel forgotPasswordNotificationModel = new(ANY_EMAIL, "fr", ANY_LINK);

        // Act
        string templateId = forgotPasswordNotificationModel.TemplateId();

        // Assert
        templateId.ShouldBe(FR_TEMPLATE_ID);
    }

    [Fact]
    public void WhenTemplateData_ThenReturnLinkAsButtonUrl()
    {
        // Arrange
        ForgotPasswordNotificationModel forgotPasswordNotificationModel = new(ANY_EMAIL, ANY_LOCALE, ANY_LINK);

        // Act
        object templateData = forgotPasswordNotificationModel.TemplateData();

        // Assert
        templateData.GetStringValueOfProperty("ButtonUrl").ShouldBe(ANY_LINK);
    }
}