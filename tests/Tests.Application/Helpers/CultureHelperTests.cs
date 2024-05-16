using System.Globalization;

using Application.Helpers;
using Application.Helpers.Exceptions;

namespace Tests.Application.Helpers;

public class CultureHelperTests
{
    private const string DEFAULT_TWO_LETTER_LANG = "fr";
    private const string CANADIAN_FRENCH_CULTURE_NAME = "fr-CA";
    private const string UNSUPPORTED_TWO_LETTER_LANG = "de";

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void GivenNullEmptyOrWhitespaceValue_WhenFormatTwoLetterCulture_ThenReturnDefaultTwoLetterLang(
        string twoLetterCulture)
    {
        // Act
        string actual = CultureHelper.FormatTwoLetterCulture(twoLetterCulture);

        // Assert
        actual.ShouldBe(DEFAULT_TWO_LETTER_LANG);
    }

    [Theory]
    [InlineData("fr")]
    [InlineData("en")]
    public void GivenSupportedTwoLetterCulture_WhenConvertTwoLetterIsoToCultureInfo_ThenMatchingCultureInfo(
        string twoLetterLang)
    {
        // Act
        CultureInfo actual = CultureHelper.ConvertTwoLetterIsoToCultureInfo(twoLetterLang);

        // Assert
        actual.TwoLetterISOLanguageName.ShouldBe(twoLetterLang);
    }

    [Theory]
    [InlineData("fr")]
    [InlineData("fR")]
    [InlineData("Fr")]
    [InlineData("FR")]
    public void GivenTwoLetterCulture_WhenFormatTwoLetterCulture_ThenLowercaseTwoLetterCulture(string twoLetterCulture)
    {
        // Act
        string actual = CultureHelper.FormatTwoLetterCulture(twoLetterCulture);

        // Assert
        actual.ShouldBe("fr");
    }

    [Fact]
    public void GivenUnsupportedCulture_WhenFormatTwoLetterCulture_ThenThrowUnsupportedCultureException()
    {
        // Act & assert
        Assert.Throws<UnsupportedCultureException>(() =>
            CultureHelper.FormatTwoLetterCulture(UNSUPPORTED_TWO_LETTER_LANG));
    }

    [Fact]
    public void WhenGetCurrentCulture_ThenShouldReturnCurrentThreadCulture()
    {
        // Act
        CultureInfo actual = CultureHelper.GetCurrentCulture();

        // Assert
        actual.ShouldBe(Thread.CurrentThread.CurrentCulture);
    }

    [Fact]
    public void WhenGetCurrentTwoLetterLang_ThenReturnTwoLetterISOLanguageName()
    {
        // Act
        string actual = CultureHelper.GetCurrentTwoLetterLang();

        // Assert
        actual.ShouldBe(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
    }

    [Fact]
    public void WhenGetDefaultCurrentCulture_ThenShouldReturnCanadianEnglishCultureInfo()
    {
        // Act
        CultureInfo actual = CultureHelper.GetDefaultCulture();

        // Assert
        actual.Name.ShouldBe(CANADIAN_FRENCH_CULTURE_NAME);
    }
}