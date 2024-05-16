using Application.Services.Notifications.Models;

using Tests.Common.Extensions;

namespace Tests.Application.Services.Notifications.Models;

public class TwoFactorAuthenticationNotificationModelTests
{
    private const string ANY_LOCALE = "fr";
    private const string EN_TEMPLATE_ID = "d-0ba3cba8d527436dbebe4ee440104d77";
    private const string FR_TEMPLATE_ID = "d-b6b894b660614a289e1d3e0e1cc81c17";
    private const string ANY_EMAIL = "john.doe@gmail.com";
    private const string ANY_CODE = "123456";


    [Fact]
    public void GivenAnyCode_WhenNewTwoFactorAuthenticationNotificationModel_ThenCodeShouldBeSameAsGivenCode()
    {
        // Arrange & act
        TwoFactorAuthenticationNotificationModel twoFactorAuthenticationNotificationModel =
            new(ANY_EMAIL, ANY_LOCALE, ANY_CODE);

        // Assert
        twoFactorAuthenticationNotificationModel.Code.ShouldBe(ANY_CODE);
    }

    [Fact]
    public void
        GivenAnyEmail_WhenNewTwoFactorAuthenticationNotificationModel_ThenDestinationEmailShouldBeSameAsGivenEmail()
    {
        // Arrange & act
        TwoFactorAuthenticationNotificationModel twoFactorAuthenticationNotificationModel =
            new(ANY_EMAIL, ANY_LOCALE, ANY_CODE);

        // Assert
        twoFactorAuthenticationNotificationModel.Destination.ShouldBe(ANY_EMAIL);
    }

    [Fact]
    public void GivenAnyLocale_WhenNewTwoFactorAuthenticationNotificationModel_ThenLocaleShouldBeSameAsGivenLocale()
    {
        // Arrange & act
        TwoFactorAuthenticationNotificationModel twoFactorAuthenticationNotificationModel =
            new(ANY_EMAIL, ANY_LOCALE, ANY_CODE);

        // Assert
        twoFactorAuthenticationNotificationModel.Locale.ShouldBe(ANY_LOCALE);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void GivenNullEmptyOrWhitespaceEmail_WhenTwoFactorAuthenticationNotificationModel_ThenThrowArgumentException(
        string email)
    {
        // Act & assert
        Assert.Throws<ArgumentException>(
            () => new TwoFactorAuthenticationNotificationModel(email, ANY_LOCALE, ANY_CODE));
    }

    [Fact]
    public void WhenLocaleIsEnglish_WhenTemplateId_ThenReturnEnglishTemplateId()
    {
        // Arrange
        TwoFactorAuthenticationNotificationModel twoFactorAuthenticationNotificationModel =
            new(ANY_EMAIL, "en", ANY_CODE);

        // Act
        string templateId = twoFactorAuthenticationNotificationModel.TemplateId();

        // Assert
        templateId.ShouldBe(EN_TEMPLATE_ID);
    }

    [Fact]
    public void WhenLocaleIsFrench_WhenTemplateId_ThenReturnFrenchTemplateId()
    {
        // Arrange
        TwoFactorAuthenticationNotificationModel twoFactorAuthenticationNotificationModel =
            new(ANY_EMAIL, "fr", ANY_CODE);

        // Act
        string templateId = twoFactorAuthenticationNotificationModel.TemplateId();

        // Assert
        templateId.ShouldBe(FR_TEMPLATE_ID);
    }

    [Fact]
    public void WhenTemplateData_ThenReturnTwoFactorCodeAsCode()
    {
        // Arrange
        TwoFactorAuthenticationNotificationModel twoFactorAuthenticationNotificationModel =
            new(ANY_EMAIL, ANY_LOCALE, ANY_CODE);

        // Act
        object templateData = twoFactorAuthenticationNotificationModel.TemplateData();

        // Assert
        templateData.GetStringValueOfProperty("Code").ShouldBe(ANY_CODE);
    }
}